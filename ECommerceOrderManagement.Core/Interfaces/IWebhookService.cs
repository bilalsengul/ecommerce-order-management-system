using System.Threading.Tasks;
using ECommerceOrderManagement.Core.Entities;

namespace ECommerceOrderManagement.Core.Interfaces
{
    public interface IWebhookService
    {
        Task SendOrderCreatedWebhookAsync(Order order);
        Task SendOrderCancelledWebhookAsync(Order order);
    }
} 