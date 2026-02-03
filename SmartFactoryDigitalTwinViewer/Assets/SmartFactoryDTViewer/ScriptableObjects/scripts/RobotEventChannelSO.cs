using System;
using UnityEngine;
[CreateAssetMenu(menuName ="Event/Robot Updated")]
//전달 통로 이벤트 브로커: 알리기만 
public class RobotEventChannelSO :ScriptableObject
{
    public Action<RobotDataModel> OnRaised;
    
    public void Raise(RobotDataModel robot)
    {
        OnRaised?.Invoke(robot);
    }
}
