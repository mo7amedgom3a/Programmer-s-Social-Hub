using System.Threading.Tasks;

namespace NotificationService.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
    }
}
