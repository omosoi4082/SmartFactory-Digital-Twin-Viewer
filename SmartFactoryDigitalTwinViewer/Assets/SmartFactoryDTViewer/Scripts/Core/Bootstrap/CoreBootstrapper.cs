using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 시스템 구성. 확장 시 RobotRegistrySO + RobotView[] 설정하면 다중 로봇 지원.
/// </summary>
public class CoreBootstrapper : MonoBehaviour
{
    [SerializeField] private RobotDataSourceConfig config;
    [Header("필수")]
    [SerializeField] private RobotEventChannelSO _robotEventChannelSO;
    [SerializeField] private RobotLivenessSystem _livenessSystem;

    [Header("Prefabs")]
    [SerializeField] private RobotView view;
    [SerializeField] private RobotMoveView modeler;

    [Header("Scene")]
    [SerializeField] private Transform viewRoot;
    [SerializeField] private Transform moveViewRoot;
    [SerializeField] private CameraFocusController _cameraFocusController;
    [SerializeField] private ListRootLayoutController _listLayoutController;

    [Header("Status (배터리 임계값 %)")]
    [SerializeField] private float warningBatteryThreshold = 30f;
    [SerializeField] private float dangerBatteryThreshold = 15f;

    private IRobotDataSource _robotDataSource;
    private CancellationTokenSource _cancellationTokenSource;
    private RobotPresenterFactory _presenterFactory;
    private RobotDataUpdateRunner _dataRunner;

    private void Awake()
    {
        ValidateRequiredReferences();

        var statusSystem = new RobotStatusSystem(warningBatteryThreshold, dangerBatteryThreshold);
        var viewPool = new RobotViewPool(view, viewRoot);
        var moveviewPool = new RobotMoveViewPool(modeler, moveViewRoot);
        var onRefreshLayout = _listLayoutController != null
            ? (Action<string>)(id => _listLayoutController.RefreshLayout(id))
            : null;
        _presenterFactory = new RobotPresenterFactory(viewPool, moveviewPool, _robotEventChannelSO, _cameraFocusController, onRefreshLayout);

        var registry = new RobotRegistry(statusSystem, _robotEventChannelSO, _presenterFactory);
        var mapper = new RobotDataMapper(registry);
        var queue = new RobotDataQueue();
        _dataRunner = new RobotDataUpdateRunner(queue, mapper);
        _cancellationTokenSource = new CancellationTokenSource();
        _livenessSystem.Initialized(registry);
        _robotDataSource = config.Create(queue);
    }

    private void Start()
    {
        _robotDataSource.StartAsync(_cancellationTokenSource.Token).Forget();
        _dataRunner.Start(_cancellationTokenSource.Token);
    }

    private async void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
        if (_robotDataSource != null)
            await _robotDataSource.StopAsync();
        _cancellationTokenSource?.Dispose();
        _presenterFactory?.Dispose();
    }

    private void ValidateRequiredReferences()
    {
        if (config == null) Debug.LogError($"[CoreBootstrapper] {nameof(config)}가 할당되지 않았습니다.");
        if (_robotEventChannelSO == null) Debug.LogError($"[CoreBootstrapper] {nameof(_robotEventChannelSO)}가 할당되지 않았습니다.");
        if (_livenessSystem == null) Debug.LogError($"[CoreBootstrapper] {nameof(_livenessSystem)}가 할당되지 않았습니다.");
        if (view == null) Debug.LogError($"[CoreBootstrapper] {nameof(view)} Prefab이 할당되지 않았습니다.");
        if (modeler == null) Debug.LogError($"[CoreBootstrapper] {nameof(modeler)} Prefab이 할당되지 않았습니다.");
        if (viewRoot == null) Debug.LogError($"[CoreBootstrapper] {nameof(viewRoot)}가 할당되지 않았습니다.");
        if (moveViewRoot == null) Debug.LogError($"[CoreBootstrapper] {nameof(moveViewRoot)}가 할당되지 않았습니다.");
    }
}
