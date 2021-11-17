namespace DesignPatterns2.Bridge
{
    public class MailSendBridge
    {
        public void SendFrom(IMessage mailProvider)
        {
            mailProvider.Send();
        }
    }
}
