using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SoBoard")]
public class SoBoard : ScriptableObject
{
    public List<BoardPiece> boardPieces = new List<BoardPiece>();

}
