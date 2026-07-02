using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventGridManager : ScriptableObject 
{
    public event Action ResetPieceGridChecked;
    public event Action ResetGridSlots;
    public event Action ActualiseBoard;
    public event Action<GameObject> PiecePlaced;
    public event Action OnePieceIsPlaced;
    public event Action TrySaveInventory;
    public event Action ResetInventory;
    public event Action<bool> SetAllPieceCanMove;
    public event Action<GameObject> AddBoardPiece;
    public event Action<GameObject> SelectRandomSlot;
    public event Action RemoveAtk;
    public event Action UnlockNextGridTier;


    public void InvokeResetPieceGridCheckedd() { ResetPieceGridChecked?.Invoke(); }
    public void InvokeResetGridSlots() { ResetGridSlots?.Invoke(); }
    public void InvokeActualiseBoard() { ActualiseBoard?.Invoke(); }

    public void InvokeTrySaveInventory() {  TrySaveInventory?.Invoke(); }

    public void InvokeResetInventory() {  ResetInventory?.Invoke(); }

    public void InvokePiecePlaced(GameObject piece)
    {
        PiecePlaced?.Invoke(piece);
        OnePieceIsPlaced?.Invoke();
    }

    public void InvokeAddBoardPiece(GameObject go) { AddBoardPiece?.Invoke(go); }
    public void InvokeSelectRandomSlot(GameObject go) { SelectRandomSlot?.Invoke(go); }
    public void InvokeSetAllPieceCanMove(bool can) { SetAllPieceCanMove?.Invoke(can); }

    public void InvokeRemoveAtk() {  RemoveAtk?.Invoke(); }
    public void InvokeUnlockNextGridTier() { UnlockNextGridTier?.Invoke(); }

}
