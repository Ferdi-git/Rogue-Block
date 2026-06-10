using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private GridSlot[] gridSlots;
    [SerializeField] private SoSaveInventory soSaveInventory;
    [SerializeField] private SOEventGridManager gridManager;
    [SerializeField] private float timeToGoBackToInventory = 0.2f;
    [SerializeField] private float delayInBetweenBackToInventory = 0.05f;
    [SerializeField] private SoBoard theBoard;
    [SerializeField] SOEventState eventState;

    private bool isReseting = false;

    private void OnEnable()
    {
        //eventState.EndOfCombat += ResetInventory;
        gridManager.TrySaveInventory += TryToSave;
        gridManager.ResetInventory += ResetInventory;
        //gridManager.OnePieceIsPlaced += TryToSave;
    }

    private void OnDisable()
    {
        //eventState.EndOfCombat -= ResetInventory;
        gridManager.TrySaveInventory -= TryToSave;
        gridManager.ResetInventory -= ResetInventory;
        //gridManager.OnePieceIsPlaced -= TryToSave;

    }
    private void Awake()
    {
        soSaveInventory.pieces.Clear();
        soSaveInventory.piecesPos.Clear();
        soSaveInventory.piecesRot.Clear();
        soSaveInventory.listBoardPiecesExist.Clear();
    }
    private void Start()
    {
        SaveGrid();
    }

    public void TryToSave()
    {
        if (isReseting) return;

        //gridManager.InvokeActualiseBoard();

        if(theBoard.boardPieces.Count != 0) return;
        //print(theBoard.boardPieces.Count);
        //print("HALLOO");
        SaveGrid();
        
    }


    [Button]
    private void SaveGrid()
    {
        soSaveInventory.pieces.Clear();
        soSaveInventory.piecesPos.Clear();
        soSaveInventory.piecesRot.Clear();

        for (int i = 0; i < gridSlots.Length; i++)
        {
            PieceInfo pieceOnSlot = gridSlots[i].GetPieceOnIt();

            if (pieceOnSlot == null || soSaveInventory.pieces.Contains(pieceOnSlot.gameObject))
                continue;

            soSaveInventory.pieces.Add(pieceOnSlot.gameObject);
            soSaveInventory.piecesPos.Add(pieceOnSlot.transform.position);
            soSaveInventory.piecesRot.Add(pieceOnSlot.transform.rotation);
        }
    }

    [Button]
    public void ResetInventory()
    {
        Debug.Log("ResetInventory called");


        isReseting = true;
        float delay = 0;

        List<int> listPieceMoved = new List<int>();


        for (int i = 0; i < soSaveInventory.pieces.Count; i++)
        {
            if (soSaveInventory.pieces[i].transform.position != soSaveInventory.piecesPos[i] || soSaveInventory.pieces[i].transform.rotation != soSaveInventory.piecesRot[i])
                listPieceMoved.Add(i);
        }



        if (listPieceMoved.Count == 0)
        {
            FinishReset(listPieceMoved);
            return;
        }

        int remaining = listPieceMoved.Count;

        print(remaining);


        for (int i = 0; i < listPieceMoved.Count; i++)
        {
            int savedIndex = listPieceMoved[i];


            soSaveInventory.pieces[savedIndex].GetComponent<PieceInfo>().Unfill();

            soSaveInventory.pieces[savedIndex].transform
                .DOMove(soSaveInventory.piecesPos[savedIndex], timeToGoBackToInventory)
                .SetDelay(delay);

            soSaveInventory.pieces[savedIndex].transform
                .DORotateQuaternion(soSaveInventory.piecesRot[savedIndex], timeToGoBackToInventory)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    soSaveInventory.pieces[savedIndex].GetComponent<PieceInfo>().SnapToGrid();
                    soSaveInventory.pieces[savedIndex].GetComponent<PieceMouvement>().ResetChild();

                    remaining--;
                    if (remaining == 0)
                    {
                        FinishReset(listPieceMoved);
                    }
                });

            //soSaveInventory.pieces[savedIndex].GetComponent<PieceMouvement>().ResetChild();

            delay += delayInBetweenBackToInventory;
        }

        DOVirtual.DelayedCall(timeToGoBackToInventory + delay + 0.5f, () =>
        {
            if (isReseting)
            {
                FinishReset(listPieceMoved);
            }
        });

    }


    private void FinishReset(List<int> piecesMoved)
    {
        gridManager.InvokeResetGridSlots();
        isReseting = false;
        TryToSave();
        gridManager.InvokeActualiseBoard();
    }




    public void EmptyInventoryGridSlots()
    {
        for (int nbr = 0; nbr < gridSlots.Length; nbr++)
        {
            gridSlots[nbr].ClearSlot();
        }
    }

}
