using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewPiece", menuName = "Pieces/Piece")]
public class SoPieces : ScriptableObject
{
    public int healthPoint;
    public GameObject prefab;
    public Sprite image;
    public PieceEffect pieceEffet;
    public List<int> ConditionValues;
    [FormerlySerializedAs("EfectValues")]
    [FormerlySerializedAs("EffectValues")]
    public List<int> BaseEffectValues;
    public List<int> TempEffectValues;
    public List<PieceColor> colors = new List<PieceColor> { PieceColor.Neutral};
    public string description;
    public bool isRepetition = false;
    public int numberManaGeneratingPerSquare;

    public enum PieceColor
    {
        Neutral,
        Red,
        Yellow,
        Blue,
    }

}
