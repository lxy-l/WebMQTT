using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Text;

namespace MQTT
{
    public class MQTTService:IDisposable
    {
        private static IMqttServer mqttServer;
        public MQTTService(string name,string pwd)
        {

            if (mqttServer==null)
            {
                try
                {
                    //验证客户端信息
                    var options = new MqttServerOptions
                    {
                        ConnectionValidator = new MqttServerConnectionValidatorDelegate(p =>
                        {
                            if (p.Username != name || p.Password != pwd)
                            {
                                p.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            }
                        })
                    };
                    options.DefaultEndpointOptions.Port = 1883;
                    mqttServer = new MqttFactory().CreateMqttServer();
                    mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(MqttNetServer_SubscribedTopic);
                    mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(MqttNetServer_UnSubscribedTopic);
                    mqttServer.UseApplicationMessageReceivedHandler(MqttServe_ApplicationMessageReceived);
                    mqttServer.UseClientConnectedHandler(MqttNetServer_ClientConnected);
                    mqttServer.UseClientDisconnectedHandler(MqttNetServer_ClientDisConnected);
                    mqttServer.StartAsync(options);
                    Console.WriteLine($"【{DateTime.Now}】:服务启动成功！");
                }
                catch (Exception e)
                {
                    Console.Write($"【{DateTime.Now}】:服务器启动失败：{e}");
                }
            }

           
        }

        /// <summary>
        /// 客户订阅
        /// </summary>
        private static void MqttNetServer_SubscribedTopic(MqttServerClientSubscribedTopicEventArgs e)
        {
            //客户端Id
            var ClientId = e.ClientId;
            var Topic = e.TopicFilter.Topic;
            Console.WriteLine($"【{DateTime.Now}】:客户端[{ClientId}]已订阅主题：{Topic}");
        }

        /// <summary>
        /// 客户取消订阅
        /// </summary>
        private static void MqttNetServer_UnSubscribedTopic(MqttServerClientUnsubscribedTopicEventArgs e)
        {
            //客户端Id
            var ClientId = e.ClientId;
            var Topic = e.TopicFilter;
            Console.WriteLine($"【{DateTime.Now}】:客户端[{ClientId}]已取消订阅主题：{Topic}");
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private static void MqttServe_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var ClientId = e.ClientId;
            var Topic = e.ApplicationMessage.Topic;
            string Payload=string.Empty;
            if (e.ApplicationMessage.Payload!=null)
            {
                 Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            }
            var Qos = e.ApplicationMessage.QualityOfServiceLevel;
            var Retain = e.ApplicationMessage.Retain;
            Console.WriteLine($"【{DateTime.Now}】:客户端[{ClientId}]>> 主题：[{Topic}] 负载：[{Payload}] Qos：[{Qos}] 保留：[{Retain}]");
        }

        /// <summary>
        /// 客户连接
        /// </summary>
        private static void MqttNetServer_ClientConnected(MqttServerClientConnectedEventArgs e)
        {
            var ClientId = e.ClientId;
            Console.WriteLine($"【{DateTime.Now}】:客户端[{ClientId}]已连接");
        }

        /// <summary>
        /// 客户连接断开
        /// </summary>
        private static void MqttNetServer_ClientDisConnected(MqttServerClientDisconnectedEventArgs e)
        {
            var ClientId = e.ClientId;
            Console.WriteLine($"【{DateTime.Now}】:客户端[{ClientId}]已断开连接");
        }

        public void Dispose()
        {
            mqttServer.StopAsync();
            mqttServer.Dispose();
        }
    }
}
