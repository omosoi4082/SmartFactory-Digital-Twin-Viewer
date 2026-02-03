using System;
using UnityEngine;

[Serializable]
public class RobotMpttDto
{
    public string robotId;
    public float battery;
    public float px,py,pz;
    public float yaw;
    public bool hasPayload;
}