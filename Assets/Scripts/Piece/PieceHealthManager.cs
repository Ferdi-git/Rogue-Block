using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu]
public class PieceHealthManager : ScriptableObject
{
    

    public int hp;
    public int shield;
    public BoardPiece piece;

    public SOEventPieceHealth pieceHealthEvent;


    public void GiveStats (int _hp, int _shield , BoardPiece boardPiece) {hp = _hp; shield = _shield;piece = boardPiece; }
    public void TakeDamage (int damage)
    {
        if (shield <= 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                Dead(damage);
            }
            else
            {
                LooseHp(damage);
            }
        }
        else
        {
            if (shield < damage)
            {
                ShieldBreak(shield);
                hp -= damage - shield;
                shield = 0;
                if (hp <= 0)
                {
                    hp = 0;
                    Dead(damage);
                }
                else
                {
                    LooseHp(damage);
                }

            }
            else
            {
                shield -= damage;
                ShieldBreak(damage);
            }
           
        }
    }

    public void LooseHp (int nbr)
    {
        ResetStatsPiece();
        pieceHealthEvent.InvokeDamage(piece,nbr);
    }

    public void ShieldBreak (int nbr)
    {
        ResetStatsPiece();
        pieceHealthEvent.InvokeShieldBreak(piece, nbr);
    }

    public void Dead (int nbr)
    {
        ResetStatsPiece();
        pieceHealthEvent.InvokeDead(piece);
    }

    private void ResetStatsPiece ()
    {
        piece.healthPoint = hp;
        piece.shield = shield;
    }
}
