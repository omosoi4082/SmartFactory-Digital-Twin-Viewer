using UnityEngine;

/// <summary>
/// 데이터 소스 생성 전담.
/// [실제 MQTT/REST 전환 시]
/// 1. 이 파일의 Create()만 수정
/// 2. FakeRobotDataSource.cs 삭제 (또는 테스트용으로 유지)
/// 3. MqttRobotDataSource.cs 등 신규 구현체 추가
/// </summary>
public static class RobotDataSourceFactory
{
   /* public static IRobotDataSource Create(RobotDataMapper mapper, string[] robotIds)
    {
        // === 테스트용 (현재) ===
     //   return new FakeRobotDataSource(mapper, robotIds);

        // === 실제 전환 시 여기만 교체 ===
        // return new MqttRobotDataSource(mapper, brokerUrl, topic);  // robotIds 불필요 (메시지에서 수신)
        // return new RestRobotDataSource(mapper, apiUrl);
    }*/
}
