using System.Threading.Tasks;

namespace ECommerceOrderManagement.Core.Interfaces
{
    public interface IMessageQueueService
    {
        Task PublishAsync<T>(string queueName, T message);
        Task SubscribeAsync<T>(string queueName, System.Func<T, Task> handler);
    }
} 