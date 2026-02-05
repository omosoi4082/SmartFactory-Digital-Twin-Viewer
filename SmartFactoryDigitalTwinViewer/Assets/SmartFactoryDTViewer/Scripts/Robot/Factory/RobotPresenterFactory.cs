using System.Collections.Generic;
using UnityEngine;

public class RobotPresenterFactory
{
    private readonly RobotViewPool _viewPool;
    private readonly RobotMoveViewPool _moveViewPool;
    private readonly RobotEventChannelSO _robotEventChannelSO;
    private Dictionary<string, RobotPresenter> _presenters = new Dictionary<string, RobotPresenter>(); //관리
    public RobotPresenterFactory(RobotViewPool robotViewPool, RobotMoveViewPool moveViewPool, RobotEventChannelSO channelSO)
    {
        _viewPool = robotViewPool;
        _moveViewPool = moveViewPool;
        _robotEventChannelSO = channelSO;
        _robotEventChannelSO.OnRaised += OnRobotUpdated;
    }

    public void PresenterGetOrCreate(RobotDataModel model)
    {
        if (_presenters.TryGetValue(model._robotId,out var pr))
        {
          Debug.Log($"Robot not found: {model._robotId}");
            return;
        }
           

        var viewModel = new RobotViewModel();
        var view = _viewPool.Get();
        var move = _moveViewPool.Get();

        var presenter = new RobotPresenter( viewModel, view, move);
        _presenters.Add(model._robotId, presenter);
    }

    private void OnRobotUpdated(RobotDataModel model)
    {
        if (_presenters.TryGetValue(model._robotId, out var robotPresenter))
        {
            robotPresenter.OnRobotUpdated(model);
        }
    }
    public void Dispose()
    {
        _robotEventChannelSO.OnRaised -= OnRobotUpdated;
    }
}
