using UnityEngine;
using UnityEngine.EventSystems;

public class MoveArrows : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Clocks clocks;
    [SerializeField] private EditTime edit;
    [SerializeField] private float Sensitivity;

    private float lastDelatX;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastDelatX = 0;
    }

    public void OnDrag(PointerEventData data)
    {
        lastDelatX += data.delta.x;
        if(lastDelatX > 1f)
        {
            lastDelatX = 0f;
            edit.ShowTime(clocks.ChangeTimeManually(Sensitivity));
        }
        else if(lastDelatX < -1f)
        {
            lastDelatX = 0f;
            edit.ShowTime(clocks.ChangeTimeManually(-Sensitivity));
        }
    }
}
