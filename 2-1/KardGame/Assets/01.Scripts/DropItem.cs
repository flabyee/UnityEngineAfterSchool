using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void DropItemMoveStartEvent(DropItem item);
    public event DropItemMoveStartEvent onMoveStart;

    public delegate void DropItemMoveEndEvent(DropItem item);
    public event DropItemMoveEndEvent onMoveEnd;

    public delegate void DropItemNoEvent(DropItem item);
    public event DropItemNoEvent onNothing;

    private RectTransform rectTransform;
    private RectTransform clampRectTransform;

    private Vector3 originalWorldPos;
    private Vector3 originalRectWorldPos;

    private Vector3 minWorldPosition;
    private Vector3 maxWorldPosition;

    public DropArea droppedArea;
    public DropArea prevDropArea;

    bool isHandle;
    bool isField;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        clampRectTransform = rectTransform.root.GetComponent<RectTransform>();

        isHandle = true;
        isField = false;
    }

    public void SetDroppedArea(DropArea dropArea)
    {
        this.droppedArea = dropArea;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalRectWorldPos = rectTransform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform,
            eventData.position, eventData.pressEventCamera, out originalWorldPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onMoveStart != null) onMoveStart(this);
        DropArea.SetDropArea(true);

        if (droppedArea != null) droppedArea.TriggerOnLift(this);
        prevDropArea = droppedArea;
        droppedArea = null;


        // 드래그 시작할 때 설정?
        Rect clamp = new Rect(Vector2.zero, clampRectTransform.rect.size);
        Vector3 minPosition = clamp.min - rectTransform.rect.min;
        Vector3 maxPosition = clamp.max - rectTransform.rect.max;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, minPosition,
            eventData.pressEventCamera, out minWorldPosition);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, maxPosition,
            eventData.pressEventCamera, out maxWorldPosition);


        //Debug.Log(minWorldPosition + "/" + maxWorldPosition);


        if(isHandle)
        {
            gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(3, 3, 0);
        }
        else if(isField)
        {
            gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.6f, 1.6f, 0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 시작할 때 설정한 값을 이용해서 이동
        Vector3 worldPointerPosition;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle (clampRectTransform, eventData.position,
            eventData.pressEventCamera, out worldPointerPosition))
        {
            Vector3 offsetToOriginal = worldPointerPosition - originalWorldPos;
            rectTransform.position = originalRectWorldPos + offsetToOriginal;
        }

        Vector3 worldPos = rectTransform.position;
        worldPos.x = Mathf.Clamp(rectTransform.position.x, minWorldPosition.x, maxWorldPosition.x);
        worldPos.y = Mathf.Clamp(rectTransform.position.y, minWorldPosition.y, maxWorldPosition.y);
        rectTransform.position = worldPos;


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DropArea.SetDropArea(false);
        if (onMoveEnd != null) onMoveEnd(this);

        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1, 1);

        bool noEvent = true;
        foreach(var go in eventData.hovered)
        {
            var dropArea = go.GetComponent<DropArea>();
            if(dropArea != null)
            {
                noEvent = false;
                break;
            }
        }

        if(noEvent)
        {
            if (onNothing != null) onNothing(this);
        }

        gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
    }

    public void IsHandle()
    {
        isHandle = true;
        isField = false;
    }

    public void IsField()
    {
        isHandle = false;
        isField = true;
    }
}
