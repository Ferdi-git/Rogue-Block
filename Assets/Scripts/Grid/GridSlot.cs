using TMPro;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public bool isFilled = false;
    public bool isAttacked;
    public TextMeshPro text;

    private GameObject pieceOnIt = null;
    private SinglePieceSquare squareOnIt = null;

    public void SetNumber(int i)
    {
        text.text = i.ToString();
    }

    public void SetPiece(GameObject piece, SinglePieceSquare square)
    {
        pieceOnIt = piece;
        squareOnIt = square;
        isFilled = piece != null;
    }

    public PieceInfo GetPieceOnIt()
    {
        if (pieceOnIt == null) return null;
        return pieceOnIt.GetComponent<PieceInfo>();
    }
    public SinglePieceSquare GetSinglePieceOnIt()
    {
        if (squareOnIt == null) return null;
        return squareOnIt;
    }

    public void ClearSlot()
    {
        pieceOnIt = null;
        isFilled = false;
    }

    public void GetSelected() => isAttacked = true;
    public void GetDeselected() => isAttacked = false;

}