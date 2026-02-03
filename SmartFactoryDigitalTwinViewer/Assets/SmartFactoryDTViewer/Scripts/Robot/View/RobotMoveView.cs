using UnityEngine;

public class RobotMoveView : MonoBehaviour
{
    public void OnRender(RobotViewModel vm)
    {
        transform.position = vm.position;  
        transform.rotation = vm.rotation;
    }
}
