using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropTestScript : MonoBehaviour
{
    public DropArea topArea;
    public DropArea bottomArea;

    public RectTransform topRectParent;
    public RectTransform bottomRectParent; 
    public RectTransform hoverRectParent;

    private void Awake()
    {
        topArea.onLifted += ObjectLiftedFromTop;
        topArea.onDropped += ObjectDroppedToTop;

        bottomArea.onLifted += ObjectLiftedFromBottom;
        bottomArea.onDropped += ObjectDroppedToBottom;
    }

    private void ObjectLiftedFromTop(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedToTop(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(topRectParent, true);
    }

    private void ObjectLiftedFromBottom(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedToBottom(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(bottomRectParent, true);
    }

    private void SetDropArea(bool active)
    {
        topArea.gameObject.SetActive(active);
        bottomArea.gameObject.SetActive(active);
    }
}
