using UnityEngine;
/// <summary>
/// 도메인 규칙
/// </summary>
public class RobotStatusSystem 
{
    private readonly float _warningBattery;
    private readonly float _dangerBattery;

    public RobotStatusSystem(float warningBattery, float dangerBattery)
    {
        _warningBattery = warningBattery;
        _dangerBattery = dangerBattery;
    }   

    public void Evaluate(RobotDataModel robot)
    {
        if(robot._batteryLevel<=_dangerBattery)
        {
            robot.SetState(RobotState.Danger);
        }
        else if(robot._batteryLevel<=_warningBattery)
        {
            robot.SetState(RobotState.Warning); 
        }
        else
        {
            robot.SetState(RobotState.Normal);
        }
    }

}
