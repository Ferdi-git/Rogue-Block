using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySave", menuName = "Save/InventorySave")]
public class SoSaveInventory : ScriptableObject
{
    public List<GameObject> pieces;
    public List<Vector3> piecesPos;
    public List<Quaternion> piecesRot;

    public List<BoardPiece> listBoardPiecesExist;

}

