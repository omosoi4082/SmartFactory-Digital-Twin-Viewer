using UnityEngine;
/// <summary>
/// MQTT / REST / Socket 같은 외부 세계 포맷을 Unity 내부에서 쓰는 RobotDataModel과 연결만 해주는 역할
/// </summary>
public class RobotDataMapper
{
    private readonly RobotRegistry _registry;
   

    public RobotDataMapper(RobotRegistry registry)
    {
        _registry = registry;
    }
    //외부 → 내부 데이터 형태 변환 DTO → Model 변환
    public void Apply(RobotMpttDto mpttDto)
    {
        _registry.UpdateRobot(
            mpttDto.robotId,
            mpttDto.battery,
            new Vector3(mpttDto.px, mpttDto.py, mpttDto.pz),
            Quaternion.Euler(0, mpttDto.yaw, 0),
            mpttDto.hasPayload
            );
       
    }
}
