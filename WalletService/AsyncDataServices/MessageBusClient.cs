using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using WalletService.Dtos;

namespace WalletService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger_Wallet", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to Message Broker");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not connect to RabbitMQ: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewWallet(WalletPublishedDto walletPublishedDto)
        {
            var message = JsonSerializer.Serialize(walletPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending...");
            }
        }
        public void PublishNewTopUp(TopUpPublishedDto topUpPublishedDto)
        {
            var message = JsonSerializer.Serialize(topUpPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger_wallet", routingKey: "",
            basicProperties: null, body: body);
            Console.WriteLine($"--> We have sent {message}");
        }
        private void SendMessageTopUp(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger_topup", routingKey: "",
            basicProperties: null, body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        
    }
}