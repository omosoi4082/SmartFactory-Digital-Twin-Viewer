
using UnityEngine;
using UnityEngine.EventSystems;
using R3;
using TMPro;
using UnityEngine.UI;
using System;

public class RobotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //메인
    public string robotId;
    [SerializeField] private RectTransform listitem;
    [SerializeField] private Button main;
    [SerializeField] private float mainHeight = 64f;
    [SerializeField] private TextMeshProUGUI robotIdtext;
    [SerializeField] private Image online;
    Color on;
    Color off;

    //상세
    [SerializeField] private GameObject details;
    [SerializeField] private float detailsHeight = 160f;
    [SerializeField] private TextMeshProUGUI px,py,pz;
    [SerializeField] private TextMeshProUGUI battery;
    [SerializeField] private Image filled;
    [SerializeField] private Image hasPayload;

    public bool isExpanded { get; private set; }
    public float currentHeight => mainHeight + (isExpanded ? detailsHeight : 0f);

    public Action<RobotView> OnClicked;
    public Action<RobotView> OnHoverEnter;
    public Action<RobotView> OnHoverExit;

    private void Start()
    {
        main.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData) => OnHoverEnter?.Invoke(this);
    public void OnPointerExit(PointerEventData eventData) => OnHoverExit?.Invoke(this);

    private void OnClick()
    {
        OnClicked?.Invoke(this);
    }
    public void SetExpended(bool exp)
    {
        isExpanded = exp;
        details.SetActive(exp);
    }

    public void Bind(RobotViewModel vmodel)
    {
        vmodel.positionText.Subscribe(value =>
        {
            px.text = value;
        }).AddTo(this);
    }
}