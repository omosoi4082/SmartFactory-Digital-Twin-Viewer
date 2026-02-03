using R3;
using UnityEngine;

public class RobotViewModel 
{
    public BindableReactiveProperty<string> positionText { get; } = new BindableReactiveProperty<string>("0,0");
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public void UpdateFromModel(RobotDataModel robot)
    {
        positionText.Value = $"{robot._position.x:0.0}, {robot._position.z:0.0}";
        position = robot._position;
        rotation = robot._rotation;
    }
}
