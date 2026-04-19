using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.StartHolding();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.Instance.StopHolding();
    }
}