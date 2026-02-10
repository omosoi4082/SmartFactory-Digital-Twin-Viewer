using UnityEngine;

/// <summary>
/// 로봇 생존 감지. timeout 초과 시 연결 끊김으로 처리.
/// </summary>
public class RobotLivenessSystem : MonoBehaviour
{
    [SerializeField] private float timeoutSeconds = 3f;
    private RobotRegistry _registry;

    public void Initialized(RobotRegistry registry)
    {
        _registry = registry;
    }

    private void Update()
    {
        if (_registry == null) return;

        float now = Time.time;
        foreach (var robot in _registry.GetAll())
        {
            if (robot.isAlive && now - robot.lastSeenTime > timeoutSeconds)
            {
                robot.MarkDisconnected();
            }
        }
    }
}
