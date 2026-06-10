using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BoardPiece
{
    public int maxHealthPoint;
    private int _healthPoint;
    public int healthPoint 
    { 
        get { return _healthPoint; }
        set
        {
            if (value > maxHealthPoint) _healthPoint = maxHealthPoint;
            else { _healthPoint = value; }
        }
    }
    public int shield;

    public SoPieces soPieces;
    public PieceInfo pieceInfo;
    public PieceAnimations pieceAnimation;
    public Context context = new();


}

