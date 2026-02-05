using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
/// <summary>
/// mqttnet.4.3.7.1207받아야함
/// MQTTnet.dll  복사 넣기
/// MQTT 연결
/// </summary>
public class MqttRobotDataSource : IRobotDataSource
{
    private readonly RobotDataQueue _queue;
    private IMqttClient _mqttClient;


    public MqttRobotDataSource(RobotDataQueue queue)
    {
        _queue = queue;
    }

    public async UniTask StartAaync(CancellationToken ct)
    {
       var factory=new MqttFactory();
        _mqttClient=factory.CreateMqttClient();

        _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;
        var options = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await _mqttClient.ConnectAsync(options, ct);
                await _mqttClient.SubscribeAsync("robots/telemetry");
                await WaitUntilDisconnected(ct);

            }
            catch(OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                await UniTask.Delay(3000, cancellationToken: ct);
            }
        }
        
    }
    private async UniTask WaitUntilDisconnected(CancellationToken ct)
    {
        while(_mqttClient.IsConnected&&!ct.IsCancellationRequested)
        {
            await UniTask.Delay(500, cancellationToken: ct);
        }
    }

    private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        var json = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        Debug.Log($"[MQTT RECEIVED] {json}");
        var dto = JsonUtility.FromJson<RobotMpttDto>(json);

        _queue.Enquene(dto);
        return Task.CompletedTask;
    }

    public async UniTask StopAaync()
    {
       if(_mqttClient != null&&_mqttClient.IsConnected) {
            await _mqttClient.DisconnectAsync();
        }
    }

  
}
