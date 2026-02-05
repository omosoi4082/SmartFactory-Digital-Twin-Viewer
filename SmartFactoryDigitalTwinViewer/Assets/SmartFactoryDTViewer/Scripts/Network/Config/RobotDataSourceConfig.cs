using UnityEngine;

public abstract class RobotDataSourceConfig : ScriptableObject
{
    public abstract IRobotDataSource Create(RobotDataQueue queue);
}
