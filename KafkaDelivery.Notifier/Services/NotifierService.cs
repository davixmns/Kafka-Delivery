namespace KafkaDelivery.Notifier.Services;

public class NotifierService : INotifierService
{
    public void Notify(string email)
    {
        Console.WriteLine("Email sent to: " + email);
    }
}