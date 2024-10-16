namespace UserManagementService.Interfaces
{
    public interface IEventProcessor
    {
        public void ProcessEvent(string message);
    }
}