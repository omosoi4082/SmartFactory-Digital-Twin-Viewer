using UnityEngine;
[CreateAssetMenu(menuName = "Robot/DataSource/MQTT", fileName = "MqttRobotDataSourceConfig")]

public class MqttRobotDataSourceConfig : RobotDataSourceConfig
{
    public override IRobotDataSource Create(RobotDataMapper mapper)
    {
        return new MqttRobotDataSource(mapper);
    }

}