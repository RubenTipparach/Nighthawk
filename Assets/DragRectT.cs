using UnityEngine;
using UnityEngine.EventSystems;

public class DragRectT : MonoBehaviour,IDragHandler{
    Canvas canvas;
    Vector2 pos;
    RectTransform rectTransform;
    public RectTransform targetRectT_;//【拖拽赋值】被拖动的窗口
    public Vector3 v3Distance;
    public bool isSetIndex = false;

    // Use this for initialization
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//transform as RectTransform;
    }

    void OnEnable()
    {
        if (targetRectT_ != null)
        {//记录拖动区域和 被拖动窗口的距离
            v3Distance = rectTransform.anchoredPosition3D - targetRectT_.anchoredPosition3D;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {//实现 当拖动UI的 接口方法
        canvas = canvas == null ? GetComponentInParent<Canvas>() : canvas;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos))
        {
            rectTransform.anchoredPosition = pos;
            if (targetRectT_ != null)
            {
                targetRectT_.anchoredPosition3D = rectTransform.anchoredPosition3D - v3Distance;//被拖动窗口的坐标等于 拖动区域坐标 减去  记录的他们之间的距离
                if (isSetIndex)
                {//将拖动窗口的层级显示在屏幕的最前面。（不被其他个别UI界面挡住）
                    targetRectT_.SetAsLastSibling();
                }
            }
        }
    }
}
