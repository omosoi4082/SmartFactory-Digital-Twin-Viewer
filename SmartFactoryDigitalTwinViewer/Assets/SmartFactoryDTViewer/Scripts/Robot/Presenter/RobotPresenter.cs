
/// <summary>
/// 로봇1대에 1개 로직 
/// 단일 
/// </summary>
public class RobotPresenter
{
    private readonly RobotViewModel _robotViewModel;
    private readonly RobotView _view;
    private readonly RobotMoveView _modeler;

    public RobotPresenter( RobotViewModel robotViewModel, RobotView view, RobotMoveView modeler)
    {
        _robotViewModel = robotViewModel;
        _view = view;
        _modeler = modeler;
    }
   

    public void OnRobotUpdated(RobotDataModel model)
    {
        _robotViewModel.UpdateFromModel(model);
        _view.Bind(_robotViewModel);
        _modeler.OnRender(_robotViewModel);
    }
}
