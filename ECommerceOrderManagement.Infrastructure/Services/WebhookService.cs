using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ECommerceOrderManagement.Core.Entities;
using ECommerceOrderManagement.Core.Interfaces;

namespace ECommerceOrderManagement.Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WebhookService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public WebhookService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<WebhookService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task SendOrderCreatedWebhookAsync(Order order)
        {
            await SendWebhookAsync("order-created", order);
        }

        public async Task SendOrderCancelledWebhookAsync(Order order)
        {
            await SendWebhookAsync("order-cancelled", order);
        }

        private async Task SendWebhookAsync<T>(string eventType, T data)
        {
            try
            {
                var webhookUrl = _configuration[$"Webhooks:{eventType}"];
                if (string.IsNullOrEmpty(webhookUrl))
                {
                    _logger.LogWarning($"No webhook URL configured for event type: {eventType}");
                    return;
                }

                var webhookData = new
                {
                    EventType = eventType,
                    Timestamp = DateTime.UtcNow,
                    Data = data
                };

                var json = JsonSerializer.Serialize(webhookData, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(webhookUrl, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to send webhook for event {eventType}. Status code: {response.StatusCode}");
                    // You might want to implement retry logic here
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending webhook for event {eventType}");
                throw;
            }
        }
    }
} 