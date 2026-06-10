using UnityEngine;

[CreateAssetMenu]
public class SoNbrOfPiecePlayed : ScriptableObject
{
    [SerializeField] private int piecePlayed;
    [SerializeField] private int pieceRepeated;

    public int GetPiecePlayed ()
    {
        return piecePlayed;
    }

    public int GetPieceRepeated ()
    {
        return pieceRepeated;
    }

    public void PiecePlayedUp ()
    {
        piecePlayed += 1;
    }

    public void RepeatedPieceUp()
    {
        pieceRepeated += 1;
    }

    public void ResetInt ()
    {
        piecePlayed = 0;
        pieceRepeated = 0;
    }
}
