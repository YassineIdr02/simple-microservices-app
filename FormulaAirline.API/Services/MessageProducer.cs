using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FormulaAirline.API.Services;

public class MessageProducer : IMessageProducer
{
    private readonly IConfiguration _configuration;

    public MessageProducer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            UserName = "user",
            Password = "pwd"
        };

        var connection = factory.CreateConnection(); // creating connection

        using var channel = connection.CreateModel(); // creating the channel 

        channel.QueueDeclare("bookings", true, true); // creating the message queue 

        var stringJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(stringJson);
        
        channel.BasicPublish("", "bookings", body:body); // send the message
        
    }
}