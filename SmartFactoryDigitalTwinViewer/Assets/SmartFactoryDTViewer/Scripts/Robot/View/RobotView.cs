    
using UnityEngine;
using R3;
using TMPro;

public class RobotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;

  
    public void Bind(RobotViewModel vmodel)
    {
        vmodel.positionText.Subscribe(value =>
        {
            position.text = value;
        }).AddTo(this);
    }
}