using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventPieceHealth : ScriptableObject
{
    public event Action<BoardPiece, int> PieceShieldBreak;
    public event Action<BoardPiece, int> PieceTakeDamage;
    public event Action<BoardPiece> PieceDie;

    public void InvokeDamage(BoardPiece piece, int dmg)
    {
        PieceTakeDamage?.Invoke(piece, dmg);
    }

    public void InvokeShieldBreak(BoardPiece piece, int dmg)
    {
        PieceShieldBreak?.Invoke(piece, dmg);
    }

    public void InvokeDead(BoardPiece piece)
    {
        PieceDie?.Invoke(piece);
    }
}
