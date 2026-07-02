using UnityEngine;
using UnityEngine.Events;

public class SimpleButton : MonoBehaviour, IMouseClickable, IMouseHoverable
{
    [SerializeField] private UnityEvent onClick;
    public SOEventGridManager eventGrid;

    private bool CanBeClicked = true;
    public bool canBeClicked
    {
        get
        {
            return CanBeClicked;
        }
        set
        {
            CanBeClicked = value;
        }
    }

    private void OnEnable()
    {
        eventGrid.SetAllPieceCanMove += SetCanBeClicked;
    }

    private void OnDisable()
    {
        eventGrid.SetAllPieceCanMove -= SetCanBeClicked;

    }

    public void OnClick()
    {
        if(!canBeClicked) {return; }
        onClick.Invoke();
    }

    public void OnHoverEnter()
    {
        //
    }

    public void OnHoverExit()
    {
        //
    }

    public void OnRightClick()
    {
        //
    }

    public void SetCanBeClicked(bool can)
    {
        canBeClicked = can;
    }
}
