using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerApp
{
    class Program
    {
        public static string GeneratePin()
        {
            string pin = string.Empty;
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                pin += r.Next(0, 10);
            }
            return pin;
        }
        public static void ProduceMessageToQueue(string message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
        }
        static void Main(string[] args)
        {
            while (true)
            {
                ProduceMessageToQueue("Hello", "messages");
                Thread.Sleep(3000);
            }
        }
    }
}
