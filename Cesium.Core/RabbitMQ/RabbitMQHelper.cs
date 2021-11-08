using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.RabbitMQ
{
    public class RabbitMQHelper
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly ILogger<RabbitMQHelper> _logger;
        public RabbitMQHelper(IRabbitMQConnection rabbitMQConnection, ILogger<RabbitMQHelper> logger)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _logger = logger;
        }

        public void Publish(string queuename, string sendmessage, Action action)
        {
            if (!_rabbitMQConnection.IsConnected)
            {
                _rabbitMQConnection.TryConnect();
            }

            using (var channel = _rabbitMQConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queuename,//队列名 
                                     durable: true,//是否持久化 
                                     exclusive: false,//true:排他性，该队列仅对首次申明它的连接可见，并在连接断开时自动删除 
                                     autoDelete: false,//true:如果该队列没有任何订阅的消费者的话，该队列会被自动删除 
                                     arguments: null);//如果安装了队列优先级插件则可以设置优先级
                // 将消息标记为持久性。
                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = Guid.NewGuid().ToString(); ;
                //properties.Expiration = "30000";
                var body = Encoding.UTF8.GetBytes(sendmessage);
                channel.BasicPublish(exchange: "",//exchange名称 
                                     routingKey: queuename,//如果存在exchange,则消息被发送到名称为hello的queue的客户端 
                                     basicProperties: properties,
                                     body: body);//消息体
            }
        }
    }
}
