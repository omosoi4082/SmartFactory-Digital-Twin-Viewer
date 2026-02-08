using System;
using UnityEngine;

public class RobotMoveView : MonoBehaviour
{
    /// <summary>아웃라인용 오브젝트(자식 또는 별도). 있으면 활성/비활성으로 표시.</summary>
    [SerializeField] private GameObject outlineDisplay;
    /// <summary>아웃라인 없을 때 Outline 컴포넌트로 토글 (유니티 Outline 있으면)</summary>
    [SerializeField] private Behaviour outlineBehaviour;

    public Action<RobotMoveView> OnClicked;
    public Action<RobotMoveView> OnHoverEnter;
    public Action<RobotMoveView> OnHoverExit;

    private void Reset()
    {
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    public void SetOutlineVisible(bool visible)
    {
        if (outlineDisplay != null)
            outlineDisplay.SetActive(visible);
        if (outlineBehaviour != null)
            outlineBehaviour.enabled = visible;
    }

    public void OnRender(RobotViewModel vm)
    {
        transform.position = vm.position;
        transform.rotation = vm.rotation;
    }

    private void OnMouseEnter() => OnHoverEnter?.Invoke(this);
    private void OnMouseExit() => OnHoverExit?.Invoke(this);

    private void OnMouseDown() => OnClicked?.Invoke(this);
}
