namespace ClassServiceAPI.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string queueName);
}