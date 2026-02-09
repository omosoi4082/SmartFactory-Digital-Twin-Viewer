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

    private IRobotDataSource _robotDataSource;
    private CancellationTokenSource cancellationTokenSource;
    private RobotPresenterFactory presenterFactory;
    private RobotDataUpdateRunner dataRunner;

    private void Awake()
    {
        var statusSystem = new RobotStatusSystem(30f, 15f);
        var viewPool = new RobotViewPool(view, viewRoot);
        var moveviewPool = new RobotMoveViewPool(modeler, moveViewRoot);
        var onRefreshLayout = _listLayoutController != null
            ? (Action<string>)(id => _listLayoutController.RefreshLayout(id))
            : null;
        presenterFactory = new RobotPresenterFactory(viewPool, moveviewPool, _robotEventChannelSO, _cameraFocusController, onRefreshLayout);

        var registry = new RobotRegistry(statusSystem, _robotEventChannelSO, presenterFactory);
        var mapper = new RobotDataMapper(registry);
        var queue = new RobotDataQueue();
        dataRunner = new RobotDataUpdateRunner(queue,mapper);
        cancellationTokenSource = new CancellationTokenSource();
        _livenessSystem.Initialized(registry);
        _robotDataSource = config.Create(queue);
    }

    private void Start()
    {
        _robotDataSource.StartAaync(cancellationTokenSource.Token).Forget();
        dataRunner.Start(cancellationTokenSource.Token);
    }

    private async void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        if (_robotDataSource != null)
            await _robotDataSource.StopAaync();
        cancellationTokenSource.Dispose();  
        presenterFactory.Dispose();
    }



}
