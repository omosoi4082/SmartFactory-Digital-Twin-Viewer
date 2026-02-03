using UnityEngine;

/// <summary>
/// 앱 전역 설정: 등록 로봇 목록 + 배터리 임계값.
/// CoreBootstrapper에서 참조하여 RobotStatusSystem / UI에 전달.
/// </summary>
[CreateAssetMenu(fileName = "RobotRegistry", menuName = "SmartFactoryDT/Robot Registry", order = 1)]
public class RobotRegistrySO : ScriptableObject
{
    [Header("배터리 임계값 (%)")]
    [Tooltip("이 값 이하면 Warning 상태")]
    [Range(0f, 100f)]
    public float warningBattery = 30f;

    [Tooltip("이 값 이하면 Danger 상태")]
    [Range(0f, 100f)]
    public float dangerBattery = 15f;

    [Header("로봇 목록 (메타데이터)")]
    [Tooltip("관제 대상 로봇 설정. 여기 없는 ID도 런타임에 데이터 오면 자동 등록됨")]
    public RobotConfigSO[] robotConfigs = new RobotConfigSO[0];

    [Header("기본값")]
    [Tooltip("목록에 없는 로봇 ID에 대한 기본 색상")]
    public Color defaultRobotColor = Color.gray;

    /// <summary>
    /// robotId에 해당하는 설정이 있으면 반환, 없으면 null.
    /// </summary>
    public RobotConfigSO GetConfig(string robotId)
    {
        if (robotConfigs == null || string.IsNullOrEmpty(robotId)) return null;
        for (int i = 0; i < robotConfigs.Length; i++)
        {
            if (robotConfigs[i] != null && robotConfigs[i].robotId == robotId)
                return robotConfigs[i];
        }
        return null;
    }

    /// <summary>
    /// robotId에 대한 표시 이름. 없으면 robotId 그대로 반환.
    /// </summary>
    public string GetDisplayName(string robotId)
    {
        var config = GetConfig(robotId);
        return config != null ? config.displayName : robotId;
    }

    /// <summary>
    /// robotId에 대한 색상. 없으면 defaultRobotColor.
    /// </summary>
    public Color GetColor(string robotId)
    {
        var config = GetConfig(robotId);
        return config != null ? config.color : defaultRobotColor;
    }
}
