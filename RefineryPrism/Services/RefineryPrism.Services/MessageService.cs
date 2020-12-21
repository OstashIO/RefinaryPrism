using RefineryPrism.Services.Interfaces;

namespace RefineryPrism.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
