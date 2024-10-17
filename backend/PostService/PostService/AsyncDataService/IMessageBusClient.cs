namespace PostService.AsyncDataService
{
    public interface IMessageBusClient
    {
        public void PublishEvent<T>(T eventMessage, string eventType);
        public void Dispose();
    }
}
