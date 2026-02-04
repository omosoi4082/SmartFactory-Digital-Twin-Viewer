using UnityEngine;
[CreateAssetMenu(menuName ="Robot/DataSource/Fake",fileName = "FakeRobotDataSourceConfig")]
public class FakeRobotDataSourceConfig : RobotDataSourceConfig
{
    public override IRobotDataSource Create(RobotDataMapper mapper)
    {
       return new FakeRobotDataSource(mapper);
    }

}
