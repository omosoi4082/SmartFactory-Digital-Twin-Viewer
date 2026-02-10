using System;
using UnityEngine;

/// <summary>
/// 로봇 원격 데이터 전송 객체 (DTO). MQTT/JSON 등 외부 포맷에서 역직렬화.
/// </summary>
[Serializable]
public class RobotMpttDto
{
    public string robotId;
    public float battery;
    public float px, py, pz;
    public float yaw;
    public bool hasPayload;
}