using DG.Tweening;
using UnityEngine;

public class PieceMouvement : MonoBehaviour, IMouseDraggable, IMouseHoverable, IMouseClickable
{

    PieceInfo pieceInfo;
    [SerializeField] PieceInfoPanel pieceInfoPanel;
    [SerializeField] GameObject anchorPanel;

    private PieceAnimations pieceAnimations;

    public bool isDraging = false;
    public bool isRotating = false;
    public bool isRotatingInputBuffer = false;

    public GameObject childThatDontTurn;

    private void Awake()
    {
        pieceInfo = GetComponent<PieceInfo>();
        pieceAnimations = GetComponent<PieceAnimations>();
    }


    public void OnDragStart(Vector2 worldPos)
    {
        isDraging = true;
        transform.DOScale(1.1f, 0.1f);
        pieceInfo.Unfill();
        pieceAnimations.ShowOnTop();
    }

    public void OnDragMove(Vector2 worldPos)
    {
        if(!isDraging) return;
        Vector2 targetPos = worldPos;
        transform.position = new Vector3(targetPos.x, targetPos.y + 0.1f, -0.1f);
    }

    public void OnDragEnd(Vector2 worldPos)
    {
        if (!isDraging) return;
        isDraging = false;
        isRotatingInputBuffer = false;
        isRotating = false;
        pieceAnimations.ShowNormal();

        transform.DOKill();

        float snappedZ = Mathf.Round(transform.eulerAngles.z / 90f) * 90f;
        transform.rotation = Quaternion.Euler(0, 0, snappedZ);

        transform.DOScale(1f, 0.1f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (pieceInfo.CheckIfCanBePlaced())
        {
            pieceInfo.SnapToGrid();
        }
        else
        {
            transform.DOMove(pieceInfo.originalPos, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                pieceInfo.Refill();
            });

            transform.DORotateQuaternion(pieceInfo.originalRota, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                childThatDontTurn.transform.DOKill();
                ResetChild();

             });

        }
    }

    public void OnHoverEnter()
    {
        transform.DOScale(1.05f, 0.1f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        //if(!isDraging)
        //{
        //    pieceInfoPanel.ShowPanel();
        //}
        pieceInfoPanel.ShowPanel();
    }

    public void OnHoverExit()
    {
        transform.DOScale(1f, 0.1f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        pieceInfoPanel.HidePanel();
    }

    public void OnClick()
    {
        // nothing
    }

    public void OnRightClick()
    {
        if (!isDraging) return;

        if (isRotating)
        {
            isRotatingInputBuffer = true;
            return;
        }

        isRotating = true;

        float currentZ = transform.eulerAngles.z;
        float targetZ = currentZ - 90f;

        ResetChild();
        transform.DORotate(new Vector3(0, 0, targetZ), 0.2f, RotateMode.FastBeyond360).OnComplete(() =>
        {

            isRotating = false;
            if (isRotatingInputBuffer)
            {
                isRotatingInputBuffer = false;
                OnRightClick();
            }
        });
    }

    public void ResetChild()
    {
        childThatDontTurn.transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);

    }


}