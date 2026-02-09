
using UnityEngine;
using UnityEngine.EventSystems;
using R3;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Splines;

public class RobotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //메인
    public string robotId;
    [SerializeField] private RectTransform listitem;
    [SerializeField] private Button main;
    [SerializeField] private float mainHeight = 64f;
    [SerializeField] private TextMeshProUGUI robotIdtext;
    [SerializeField] private Image online;
    [SerializeField] Color on;
    [SerializeField] Color off;

    //상세
    [SerializeField] private GameObject details;
    [SerializeField] private float detailsHeight = 160f;
    [SerializeField] private TextMeshProUGUI px,pz;
    [SerializeField] private TextMeshProUGUI rotation;
    [SerializeField] private TextMeshProUGUI battery;
    [SerializeField] private Image filled;
    [SerializeField] private Image hasPayload;

    public bool isExpanded { get; private set; }
    public float currentHeight => mainHeight + (isExpanded ? detailsHeight : 0f);

    public Action<RobotView> OnClicked;
    public Action<RobotView> OnHoverEnter;
    public Action<RobotView> OnHoverExit;

    bool preAlive;
    bool preHasPayload;

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
        vmodel.robotdata.Subscribe(value =>
        {
            robotIdtext.text = value._robotId;
            px.text = value._position.x.ToString("F1");
            pz.text = value._position.z.ToString("F1");
            rotation.text = value._rotation.ToString("F1");

            if(preAlive!= value.isAlive)
            {
                online.color = value.isAlive ? on : off;
                preAlive = value.isAlive;
            }
            if (preHasPayload != value._hasPayload)
            {
                hasPayload.color = value._hasPayload ? on : off;
                preHasPayload = value._hasPayload;
            }

            battery.text=value._batteryLevel.ToString("F1");
            filled.fillAmount = value._batteryLevel / 100f;

        }).AddTo(this);
    }
}