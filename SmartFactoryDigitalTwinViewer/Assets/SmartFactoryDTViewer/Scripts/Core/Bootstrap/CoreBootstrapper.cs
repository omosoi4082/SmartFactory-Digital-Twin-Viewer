using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 시스템 구성. 확장 시 RobotRegistrySO + RobotView[] 설정하면 다중 로봇 지원.
/// </summary>
public class CoreBootstrapper : MonoBehaviour
{
    private IRobotDataSource _robotDataSource;
    [Header("필수")]
    [SerializeField] private RobotEventChannelSO _robotEventChannelSO;
  
    [Header("Prefabs")]
    [SerializeField] private RobotView view;
    [SerializeField] private RobotMoveView modeler;

    [Header("Scene")]
    [SerializeField] private Transform viewRoot;
    [SerializeField] private Transform moveViewRoot;

    private RobotPresenterFactory presenterFactory;
    private void Awake()
    {
        var statusSystem = new RobotStatusSystem(30f, 15f);
        var viewPool = new RobotViewPool(view, viewRoot);
        var moveviewPool = new RobotMoveViewPool(modeler, moveViewRoot);
        presenterFactory = new RobotPresenterFactory(viewPool, moveviewPool, _robotEventChannelSO);

        var registry = new RobotRegistry(statusSystem, _robotEventChannelSO, presenterFactory);
        var mapper = new RobotDataMapper(registry);
      
        _robotDataSource = new FakeRobotDataSource(mapper);
    }

    private void Start()
    {
        _robotDataSource.RobotStart();
    }

    private void OnDestroy()
    {
        _robotDataSource.RobotStop();
        presenterFactory.Dispose();
    }

   

}
