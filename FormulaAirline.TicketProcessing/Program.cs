// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "RabbitMQHost",
    UserName = "user",
    Password = "pwd"
};

var connection = factory.CreateConnection(); // creating connection

using var channel = connection.CreateModel(); // creating the channel 

channel.QueueDeclare("bookings", true, true); // creating the message queue 

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"The message is {message}");
};

channel.BasicConsume("booking", true, consumer);
Console.ReadKey();