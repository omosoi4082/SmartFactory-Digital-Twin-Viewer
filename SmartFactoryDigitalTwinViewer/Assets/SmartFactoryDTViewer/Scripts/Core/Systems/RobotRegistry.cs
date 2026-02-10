using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model 관리
/// 로봇을 보관하고
/// 로봇을 갱신하고
/// 로봇이 변경되었음을 외부에 알림 (이벤트 기반)
/// </summary>
public class RobotRegistry
{
    private readonly Dictionary<string, RobotDataModel> robots = new Dictionary<string, RobotDataModel>();
    private readonly RobotStatusSystem _statusSystem;
    private readonly RobotEventChannelSO _eventChannelSO;
    private readonly RobotPresenterFactory _robotPresenterFactory;

    public RobotRegistry(RobotStatusSystem statusSystem, RobotEventChannelSO robotEventChannelSO, RobotPresenterFactory robotPresenter)
    {
        _statusSystem = statusSystem;
        _eventChannelSO = robotEventChannelSO;
        _robotPresenterFactory = robotPresenter;
    }

    public RobotDataModel GetOrCreate(string robotId)
    {
        if (robots.TryGetValue(robotId, out var existing))
            return existing;

        var robot = new RobotDataModel(robotId);
        robots.Add(robotId, robot);
        return robot;
    }

    /// <summary>
    /// 내부 도메인 모델을 갱신하는 공식 진입점
    /// DTO → RobotDataModel 매핑 + 상태 평가 + 이벤트 브로드캐스트
    /// </summary>
    public void UpdateRobot(string robotId, float battery, Vector3 vector3, Quaternion rotation, bool hasPayload)
    {
        var robot = GetOrCreate(robotId);
        _robotPresenterFactory.PresenterGetOrCreate(robot);
        // 센서 데이터 반영
        robot.UpdateSensorData(battery, vector3, rotation, hasPayload);

        // 배터리 수준에 따라 상태 계산
        _statusSystem.Evaluate(robot);

        // UI / ViewModel 에게 변경 알림
        _eventChannelSO.Raise(robot);
    }

    public IEnumerable<RobotDataModel> GetAll() => robots.Values;

}
