using System;
using UnityEngine;
public enum RobotState
{
    Normal,
    Warning,
    Danger
}
//로봇 상태 Model
public class RobotDataModel
{
    public string _robotId { get; }
    public float _batteryLevel { get; private set; }
    public Vector3 _position { get; private set; }
    public Quaternion _rotation { get; private set; } 
    public bool _hasPayload { get; private set; }  

    public  RobotState _state { get; private set; }
    public float lastSeenTime { get; private set; }
    public bool isAlive { get; private set; }

    public Action<RobotDataModel> onChanged;

    public RobotDataModel(string robotId)
    {
        _robotId = robotId;
        isAlive=false;
    }
    //외부 데이터(DTO)를 내부 도메인 모델로 변환
    public void UpdateSensorData(float battery, Vector3 vector3, Quaternion rotation, bool hasPayload)
    {
        _batteryLevel = battery;
        _position = vector3;
        _rotation = rotation;
        _hasPayload = hasPayload;

        lastSeenTime = Time.time;   
        isAlive = true;
        // 모델 내부 상태가 바뀌었음을 알림 → ViewModel / Presenter 가 구독
        onChanged?.Invoke(this);
    }
    public void SetState(RobotState newState)
    {
        if (_state == newState) return;
        _state = newState;

        // 상태 변경 알림
        onChanged?.Invoke(this);
    }

    public void MarkDisconnected()
    {
        if (!isAlive) return;
        isAlive = false;
        onChanged?.Invoke(this);
    }
}
