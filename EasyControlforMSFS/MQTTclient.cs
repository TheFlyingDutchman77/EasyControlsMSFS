using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace EasyControlforMSFS
{
    public class MQTTclient
    {
        public event EventHandler<string> LogResult = null;
        public string title = "";
        public MqttClient client;

        public MQTTclient()
        {
            // create client instance
            client = new MqttClient("192.168.0.137");
            try
            {
                // register to message received
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);

                // subscribe to the topic "/home/temperature" with QoS 2
                client.Subscribe(new string[] { "msfs/settrim" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.Subscribe(new string[] { "msfs/aileron_trim" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.Subscribe(new string[] { "msfs/rudder_trim" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }   
            catch (Exception ex)
            {
                Debug.WriteLine("MQTT NOT connected");
            }
            if (client.IsConnected) {Debug.WriteLine("MQTT connected"); }
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            int value = Int32.Parse(Encoding.Default.GetString(e.Message));
            Debug.WriteLine($"Message received in topic {topic} with value {value}");
            ProcessMessageReceived(topic, value);
            LogResult?.Invoke(this, $"Message received, topic: {topic}, value: {value} ");
        }

        public void SetTitle(string title_input)
        {
            title = title_input;
            Debug.WriteLine($"Title for mqtt set to {title}");
        }

        public void PublishMessage(string message, string value)
        {
            client.Publish(message, Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void ProcessMessageReceived(string topic, int value)
        {
            if (title.Contains("Boeing 247D"))
            {
                if (topic == "msfs/XXXsettrim") /// NOT USED ANYMORE
                {
                    string Lvar = "ELEVATOR TRIM";
                    int current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(Lvar);
                    if (value == 1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value - 1); }
                    else
                    {
                        if (value == -1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value + 1); }
                    }
                }
                if (topic == "msfs/XXXXaileron_trim") /// NOT USED ANYMORE
                {
                    string Lvar = "AILERON TRIM";
                    int current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(Lvar);
                    if (value == 1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value - 1); }
                    else
                    {
                        if (value == -1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value + 1); }
                    }
                }
                if (topic == "msfs/XXXXrudder_trim") /// NOT USED ANYMORE
                {
                    string Lvar = "RUDDER TRIM";
                    int current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(Lvar);
                    if (value == 1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value - 1); }
                    else
                    {
                        if (value == -1) { MainWindow.myMSFSVarServices.VS_EventSet(Lvar, current_value + 1); }
                    }
                }

            }

        }


    }
}
