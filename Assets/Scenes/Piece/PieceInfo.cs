using UnityEngine;
using UnityEngine.Audio;

public class PieceInfo : MonoBehaviour
{

    public SoPieces soPiece;
    [SerializeField] private SinglePieceSquare[] singleSquares;
    [SerializeField] private Transform[] surroundingPoints;


    AudioSource audioSource;
    [SerializeField] AudioClip snapSound;
    [SerializeField] SOEventGridManager eventGrid;
    [SerializeField] SOEventGridManager sOEventGridManager;

    public bool wasGridChecked = false;

    [SerializeField] private LayerMask gridLayer;

    public BoardPiece currentBoardPiece;


    [HideInInspector] public Vector3 originalPos;
    [HideInInspector] public Quaternion originalRota;

    private void Awake()
    {
        BoardPiece newBoardPiece = new();
        newBoardPiece.pieceAnimation = gameObject.GetComponent<PieceAnimations>();

        currentBoardPiece = newBoardPiece;
        newBoardPiece.pieceInfo = this;
        newBoardPiece.soPieces = soPiece;

        newBoardPiece.maxHealthPoint = soPiece.healthPoint;
        newBoardPiece.healthPoint = soPiece.healthPoint;

    }

    private void Start()
    {
        SnapToGrid();
        audioSource = GetComponent<AudioSource>();
        originalPos = transform.position;
        originalRota = transform.rotation;  
    }

    private void OnEnable()
    {
        sOEventGridManager.ResetPieceGridChecked += ResetPiece;
    }

    private void OnDisable()
    {
        sOEventGridManager.ResetPieceGridChecked -= ResetPiece;

    }

    public void Unfill()
    {
        foreach (var c in singleSquares)
        {
            foreach (var hit in Physics2D.OverlapPointAll(c.transform.position, gridLayer))
            {
                GridSlot slot = hit.GetComponent<GridSlot>();
                if (slot != null) { slot.ClearSlot(); break; }
            }
        }
    }

    public void Refill()
    {
        foreach (var c in singleSquares)
        {
            foreach (var hit in Physics2D.OverlapPointAll(c.transform.position, gridLayer))
            {
                GridSlot slot = hit.GetComponent<GridSlot>();
                if (slot != null) { slot.SetPiece(gameObject,c); break; }
            }
        }
    }

    public void SnapToGrid()
    {
        GridSlot targetSlot = null;
        Vector3 targetSlotPos = Vector3.zero;

        Vector2 samplePos = (Vector2)singleSquares[0].transform.position;

        foreach (var hit in Physics2D.OverlapPointAll(samplePos, gridLayer))
        {
            GridSlot slot = hit.GetComponent<GridSlot>();
            if (slot != null && !slot.isFilled)
            {
                targetSlot = slot;
                targetSlotPos = slot.transform.position;
                break;
            }
        }

        if (targetSlot == null) return;

        transform.position = targetSlotPos;

        foreach (var c in singleSquares)
        {
            foreach (var hit in Physics2D.OverlapPointAll(c.transform.position, gridLayer))
            {
                GridSlot slot = hit.GetComponent<GridSlot>();
                if (slot != null && !slot.isFilled) { slot.SetPiece(gameObject, c); break; }
            }
        }
        float randStartPitch = Random.Range(1.0f, 1.2f);
        if (audioSource != null)
        {
            audioSource.pitch = randStartPitch;
            audioSource.clip = snapSound;
            audioSource.Play();
        }
        originalPos = transform.position;
        originalRota = transform.rotation;
        eventGrid.InvokePiecePlaced(this.gameObject);

    }


    public bool CheckIfCanBePlaced()
    {
        foreach (var c in singleSquares)
            if (!CheckIfSingleCaseCanBePlaced(c.transform)) return false;

        return true;
    }

    public bool CheckIfSingleCaseCanBePlaced(Transform pos)
    {
        foreach (var hit in Physics2D.OverlapPointAll(pos.position, gridLayer))
        {
            GridSlot slot = hit.GetComponent<GridSlot>();
            if (slot != null && !slot.isFilled) return true;
        }

        return false;
    }

    public Transform[] GetSurroundingPoints()
    {
        return surroundingPoints;
    }
    public SinglePieceSquare[] GetSelfPoints()
    {
        return singleSquares;
    }

    private void ResetPiece()
    {
        wasGridChecked = false;
    }


}
