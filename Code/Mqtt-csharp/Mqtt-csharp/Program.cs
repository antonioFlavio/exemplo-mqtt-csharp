using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Mqtt_csharp
{
    class Program
    {
        static MqttClient client;

        static void Main(string[] args)
        {
            bool quit = false;
                        
            // O endereço do seu server. Estou utilizando o ActiveMQ.
            string brokerAddress = "192.168.43.223";
            client = new MqttClient(brokerAddress);

            // Assina evento para receber as mensagens do tópico;
            //client.MqttMsgPublishReceived += callbackReceiveMessage;

            var clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // Assina um tópico para leitura.
            client.Subscribe(new string[] { "csharp/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            
            // Loop para interação com o console.
            while (!quit)
            {
                var message = Console.ReadLine();

                if (message.Equals("quit"))
                {
                    sendData("i'm leaving ...");

                    quit = true;                    
                    client.Disconnect();
                }
                else
                {
                    sendData(message);
                }                
            }
        }

        private static void sendData(string message)
        {
            // Tópico para envio.
            string topic = "hello/test-antonio";

            // Envia a mensagem
            client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);            
        }

        private static void callbackReceiveMessage(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);

            Console.WriteLine(ReceivedMessage);
        }
    }
}
