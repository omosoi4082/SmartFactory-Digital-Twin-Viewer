using UnityEngine;

/// <summary>
/// 메인 카메라를 대상 위치로 이동·줌. 리스트에서 로봇 선택 시 호출.
/// </summary>
public class CameraFocusController : MonoBehaviour, ICameraFocus
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float zoomDistance = 8f;
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 _targetPosition;
    private bool _hasTarget;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void Focus(Vector3 worldPosition)
    {
        _targetPosition = worldPosition + Vector3.up * heightOffset;
        _hasTarget = true;
    }

    private void LateUpdate()
    {
        if (!_hasTarget || targetCamera == null) return;

        var cam = targetCamera.transform;
        var desiredPosition = _targetPosition - (Vector3.forward * zoomDistance) + (Vector3.up * zoomDistance * 0.3f);
        cam.position = Vector3.Lerp(cam.position, desiredPosition, moveSpeed * Time.deltaTime);
        cam.LookAt(_targetPosition);

        if ((cam.position - desiredPosition).sqrMagnitude < 0.01f)
            _hasTarget = false;
    }
}
