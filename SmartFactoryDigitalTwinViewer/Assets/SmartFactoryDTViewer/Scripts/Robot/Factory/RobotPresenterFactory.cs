using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotPresenterFactory
{
    private readonly RobotViewPool _viewPool;
    private readonly RobotMoveViewPool _moveViewPool;
    private readonly RobotEventChannelSO _robotEventChannelSO;
    private readonly ICameraFocus _cameraFocus;
    private readonly Action<string> _onRequestRefreshLayout;
    private Dictionary<string, RobotPresenter> _presenters = new Dictionary<string, RobotPresenter>();

    public RobotPresenterFactory(
        RobotViewPool robotViewPool,
        RobotMoveViewPool moveViewPool,
        RobotEventChannelSO channelSO,
        ICameraFocus cameraFocus,
        Action<string> onRequestRefreshLayout)
    {
        _viewPool = robotViewPool;
        _moveViewPool = moveViewPool;
        _robotEventChannelSO = channelSO;
        _cameraFocus = cameraFocus;
        _onRequestRefreshLayout = onRequestRefreshLayout;
        _robotEventChannelSO.OnRaised += OnRobotUpdated;
    }

    public void PresenterGetOrCreate(RobotDataModel model)
    {
        if (_presenters.TryGetValue(model._robotId, out _))
        {
            return;
        }

        var viewModel = new RobotViewModel();
        var view = _viewPool.Get();
        view.robotId = model._robotId;
        var move = _moveViewPool.Get();

        var presenter = new RobotPresenter(
            model._robotId,
            viewModel,
            view,
            move,
            _cameraFocus,
            _onRequestRefreshLayout);
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

