using R3;
using UnityEngine;

public class RobotViewModel 
{
    public BindableReactiveProperty<RobotDataModel> robotdata { get; } = new BindableReactiveProperty<RobotDataModel>();
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public string id { get; set; }
    public void UpdateFromModel(RobotDataModel robot)
    {
        // positionText.Value = $"{robot._position.x:0.0}, {robot._position.z:0.0}";
        robotdata.Value = robot;
        position = robot._position;
        rotation = robot._rotation;
        id = robot._robotId;
    }
}
