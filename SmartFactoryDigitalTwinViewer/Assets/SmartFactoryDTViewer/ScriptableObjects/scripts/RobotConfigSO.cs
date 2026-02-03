using UnityEngine;

/// <summary>
/// 단일 로봇의 메타데이터 (에디터에서 설정, 런타임에는 읽기 전용).
/// UI 표시명, 색상, 아이콘 등에 사용.
/// </summary>
[CreateAssetMenu(fileName = "RobotConfig", menuName = "SmartFactoryDT/Robot Config", order = 0)]
public class RobotConfigSO : ScriptableObject
{
    [Tooltip("MQTT/API 등에서 오는 로봇 ID와 일치해야 함")]
    public string robotId = "R-01";

    [Tooltip("관제 화면에 표시할 이름")]
    public string displayName = "로봇 1";

    [Tooltip("상태 색상/아이콘 테마에 사용 (기본 Normal 색)")]
    public Color color = Color.white;

    [Tooltip("목록/카드 UI용 아이콘 (선택)")]
    public Sprite icon;

    /// <summary>
    /// robotId가 비어 있으면 name 기반으로 사용 (에디터 전용)
    /// </summary>
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(robotId) && !string.IsNullOrEmpty(name))
            robotId = name;
    }
}
