using System;

/// <summary>
/// 로봇1대에 1개 로직. View ↔ Move 호버·클릭 연동.
/// </summary>
public class RobotPresenter
{
    private readonly string _robotId;
    private readonly RobotViewModel _robotViewModel;
    private readonly RobotView _view;
    private readonly RobotMoveView _modeler;
    private readonly ICameraFocus _cameraFocus;
    private readonly Action<string> _onRequestRefreshLayout;

    private bool _viewHovered;
    private bool _moveHovered;

    private bool _select;
    public RobotPresenter(
        string robotId,
        RobotViewModel robotViewModel,
        RobotView view,
        RobotMoveView modeler,
        ICameraFocus cameraFocus,
        Action<string> onRequestRefreshLayout)
    {
        _robotId = robotId;
        _robotViewModel = robotViewModel;
        _view = view;
        _modeler = modeler;
        _cameraFocus = cameraFocus;
        _onRequestRefreshLayout = onRequestRefreshLayout;

        _view.OnHoverEnter += _ => { _viewHovered = true; UpdateOutline(); };
        _view.OnHoverExit += _ => { _viewHovered = false; UpdateOutline(); };
        _view.OnClicked += _ => OnViewClicked();

        _modeler.OnHoverEnter += _ => { _moveHovered = true; UpdateOutline(); };
        _modeler.OnHoverExit += _ => { _moveHovered = false; UpdateOutline(); };
        _modeler.OnClicked += _ => OnViewClicked();
    }

    private void UpdateOutline()
    {
        if (_select) return;
        _modeler.SetOutlineVisible(_viewHovered || _moveHovered);
    }

    private void OnViewClicked()
    {
        _select = !_select;
        if (_select)
        {
            _cameraFocus?.Focus(_modeler.transform.position);
            _modeler.SetOutlineVisible(true);
            _view.SetExpended(true);
            _onRequestRefreshLayout?.Invoke(_robotId);
        }
        else
        {
            _modeler.SetOutlineVisible(false);
            _view.SetExpended(false);
            _onRequestRefreshLayout?.Invoke(_robotId);
        }
      
    }

    public void OnRobotUpdated(RobotDataModel model)
    {
        _robotViewModel.UpdateFromModel(model);
        _view.Bind(_robotViewModel);
        _modeler.OnRender(_robotViewModel);
    }
}
