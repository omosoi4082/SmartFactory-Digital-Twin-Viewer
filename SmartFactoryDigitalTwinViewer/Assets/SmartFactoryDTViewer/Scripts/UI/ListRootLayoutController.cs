using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListRootLayoutController : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private float spacing = 8f;

    public void RefreshLayout(string robotId)
    {
        var items = new List<RobotView>();
        for (int i = 0; i < content.childCount; i++)
        {
            var item = content.GetChild(i).GetComponent<RobotView>();
            if (item != null)
                items.Add(item);
        }

        float totalHeight = 0f;
        foreach (var item in items)
        {
            totalHeight += item.currentHeight + spacing;
        }

        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }
}
