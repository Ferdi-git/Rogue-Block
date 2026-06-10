using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GridSlot> gridSlots;
    [SerializeField] private SOEventGridManager gridManager;
    [SerializeField] private SOEventPieceHealth healthManager;
    [SerializeField] private SOEventState soEventState;
    [SerializeField] private SOEventEndPlayerTurn sOEventEndPlayer;

    [SerializeField] private SoBoard theBoard;

    [SerializeField] private SoSaveInventory soSaveInventory;
    [SerializeField] private PieceInfo[] piecesExist;

    public List<GameObject> listBoardPrefabAtk ;

    [SerializeField] TierUnlockGrid[] TiersUnlockGrid;

    private int currentGridUnlock = -1;

    public SortMode baseSortMode;

    public enum SortMode { ByRow, ByColumn }


    private void OnEnable()
    {
        gridManager.ActualiseBoard += ActualiseBoard;
        gridManager.ResetGridSlots += ResetGridSlots;
        gridManager.OnePieceIsPlaced += ActualiseBoard;
        gridManager.AddBoardPiece += AddBoardPiece;
        gridManager.SelectRandomSlot += SelectRandomSlot;
        gridManager.RemoveAtk += RemoveAtk;
        gridManager.UnlockNextGridTier += UnlockNextGridTier;
        healthManager.PieceDie += DestroyPiece;
        soEventState.StartShoping += RemoveAtk;
        sOEventEndPlayer.EndTurn += ActualiseBoard;
    }

    private void OnDisable()
    {
        gridManager.ActualiseBoard -= ActualiseBoard;
        gridManager.ResetGridSlots -= ResetGridSlots;
        gridManager.OnePieceIsPlaced -= ActualiseBoard;
        gridManager.AddBoardPiece -= AddBoardPiece;
        gridManager.SelectRandomSlot -= SelectRandomSlot;
        gridManager.RemoveAtk -= RemoveAtk;
        gridManager.UnlockNextGridTier -= UnlockNextGridTier;
        healthManager.PieceDie -= DestroyPiece;
        soEventState.StartShoping -= ActualiseBoard;
        soEventState.StartShoping -= RemoveAtk;
        sOEventEndPlayer.EndTurn -= ActualiseBoard;

    }



    private void Awake()
    {
        SortBoard(baseSortMode);

    }

    private void Start()
    {
        soSaveInventory.listBoardPiecesExist.Clear();
        for (int i = 0; i < piecesExist.Length; i++)
        {
            AddBoardPiece(piecesExist[i].gameObject);
            PieceInfo pieceInfo = piecesExist[i].GetComponent<PieceInfo>();
            
            pieceInfo.soPiece.TempEffectValues = new List<int>(pieceInfo.soPiece.BaseEffectValues);
        }
        ActualiseBoard();
    }

    [Button]
    private void ActualiseBoard()
    {
        gridManager.InvokeResetPieceGridCheckedd();
        SortBoard(baseSortMode);
        theBoard.boardPieces.Clear();
        ResetNbrAtckCase();
        SetNbrAtckCase();
        for (int i = 0; i < gridSlots.Count; i++)
        {
            gridSlots[i].SetNumber(i  + 1);
            PieceInfo pieceOnSlot = gridSlots[i].GetPieceOnIt();

            if (pieceOnSlot == null || pieceOnSlot.wasGridChecked)
                continue;


            pieceOnSlot.wasGridChecked = true;

            BoardPiece currentBoardPiece = GetBoardPiece(pieceOnSlot);
            ContextAutour newContext = GetContextAutour(pieceOnSlot);
            currentBoardPiece.context.voisins = newContext.voisins;
            currentBoardPiece.context.NbrCaseLibre = newContext.nbrCaseLibre;
            currentBoardPiece.context.NbrCaseOccupe = newContext.nbrCaseOccupe;

            theBoard.boardPieces.Add(currentBoardPiece);
        }
        gridManager.InvokeTrySaveInventory();

    }
    
    private ContextAutour GetContextAutour(PieceInfo piecePerso)
    {
        ContextAutour contextAutour =  new ContextAutour();
        for (int i = 0; i < piecePerso.GetSurroundingPoints().Length; i++)
        {
            var hits = Physics2D.OverlapPointAll(piecePerso.GetSurroundingPoints()[i].transform.position);

            if (hits.Length == 0)
            {
                contextAutour.nbrCaseOccupe += 1;
                continue;
            }

            foreach (var hit in hits)
            {
                var gridSlotVoisin = hit.gameObject.GetComponent<GridSlot>();
                if (gridSlotVoisin != null && gridSlotVoisin.GetPieceOnIt())
                {
                    if (!contextAutour.voisins.Contains(GetBoardPiece(gridSlotVoisin.GetPieceOnIt())))
                        contextAutour.voisins.Add(GetBoardPiece(gridSlotVoisin.GetPieceOnIt()));

                    contextAutour.nbrCaseOccupe += 1;
                }
                else if (gridSlotVoisin != null && !gridSlotVoisin.GetPieceOnIt())
                {
                    contextAutour.nbrCaseLibre += 1;
                }
            }
        }

        //print(contextAutour.nbrCaseOccupe + contextAutour.nbrCaseLibre.ToString());


        contextAutour.voisins = baseSortMode == SortMode.ByRow
      ? contextAutour.voisins.OrderBy(p => -p.pieceInfo.transform.position.y).ThenBy(p => p.pieceInfo.transform.position.x).ToList()
      : contextAutour.voisins.OrderBy(p => p.pieceInfo.transform.position.x).ThenBy(p => -p.pieceInfo.transform.position.y).ToList();

        return contextAutour;
    }




    private BoardPiece GetBoardPiece(PieceInfo pieceInfo)
    {
        for (int nbr = 0; nbr < soSaveInventory.listBoardPiecesExist.Count; nbr++)
        {
            if (soSaveInventory.listBoardPiecesExist[nbr].pieceInfo == pieceInfo)
            {
                return soSaveInventory.listBoardPiecesExist[nbr];
            }
        }
        Debug.LogError("Weird board piece doesnt exist");
        return null;
    }

    private void ResetNbrAtckCase()
    {
        print("Reset");

        for (int i = 0; i < soSaveInventory.listBoardPiecesExist.Count; i++)
        {
            BoardPiece bp = soSaveInventory.listBoardPiecesExist[i].pieceInfo.currentBoardPiece;
            bp.context.NbrCaseAtk = 0;
            bp.context.NbrCaseGenerateMana = 0;

        }
    }

    private void SetNbrAtckCase()
    {
        print("Set");
        for (int i = 0; i < gridSlots.Count; i++)
        {
            PieceInfo piece = gridSlots[i].GetPieceOnIt();
            if (piece != null && gridSlots[i].isAttacked)
            {
                BoardPiece bp = GetBoardPiece(piece);
                SinglePieceSquare singlePieceSquare = gridSlots[i].GetSinglePieceOnIt();

                if (singlePieceSquare != null && singlePieceSquare.generateMana) bp.context.NbrCaseGenerateMana += 1;

                bp.context.NbrCaseAtk += 1;
            }
        }

    }



    private void SortBoard(SortMode sortMode)
    {
        gridSlots = sortMode == SortMode.ByRow
            ? gridSlots.OrderBy(p => -p.transform.position.y).ThenBy(p => p.transform.position.x).ToList()
            : gridSlots.OrderBy(p => p.transform.position.x).ThenBy(p => -p.transform.position.y).ToList();
    }


    public void ResetGridSlots()
    {
        for(int nbr = 0; nbr < gridSlots.Count; nbr++)
        {
            gridSlots[nbr].ClearSlot();
        }
    }


    public void SelectRandomSlot(GameObject basePrefabAtk)
    {
        GameObject prefabAtk = Instantiate(basePrefabAtk);
        listBoardPrefabAtk.Add(prefabAtk);

        EnemyZoneAtk enemyAtk = prefabAtk.GetComponent<EnemyZoneAtk>();


        int randInt = UnityEngine.Random.Range(0, gridSlots.Count);
        int randIntRota = UnityEngine.Random.Range(0, 4);

        prefabAtk.transform.position = gridSlots[randInt].transform.position;
        prefabAtk.transform.position = gridSlots[randInt].transform.position;


        while (!enemyAtk.CheckIfCanBePlaced())
        {
            randInt = UnityEngine.Random.Range(0, gridSlots.Count);
            randIntRota = UnityEngine.Random.Range(0, 4);
            prefabAtk.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 90f * randIntRota));
            prefabAtk.transform.position = gridSlots[randInt].transform.position;
        }

        enemyAtk.SetAtk();


    }

    public void RemoveAtk()
    {
        for (int i = 0; i < listBoardPrefabAtk.Count; i++)
        {
            listBoardPrefabAtk[i].GetComponent<EnemyZoneAtk>().RemoveAtk();
            Destroy(listBoardPrefabAtk[i].gameObject);
        }
        listBoardPrefabAtk.Clear();
    }
        

    public void AddBoardPiece(GameObject go)
    {
        soSaveInventory.listBoardPiecesExist.Add(go.GetComponent<PieceInfo>().currentBoardPiece);
    }
    
    public void RemoveBoardPiece(BoardPiece bp)
    {
        for (int i = 0; i <soSaveInventory.listBoardPiecesExist.Count; i++)
        {
            if (soSaveInventory.listBoardPiecesExist[i] == bp)
            {
                soSaveInventory.listBoardPiecesExist.RemoveAt(i);

            }
        }

        for (int i = 0; i < soSaveInventory.pieces.Count; i++)
        {
            if (soSaveInventory.pieces[i] == bp.pieceInfo.gameObject)
            {
                soSaveInventory.pieces.RemoveAt(i);
                soSaveInventory.piecesPos.RemoveAt(i);
                soSaveInventory.piecesRot.RemoveAt(i);
            }
        }

    }

    private void DestroyPiece(BoardPiece bp)
    {
        bp.pieceAnimation.DestroyPieceAnim();
        RemoveBoardPiece(bp);
        ActualiseBoard();
    }

    [Button]
    private void UnlockNextGridTier()
    {
        currentGridUnlock++; 
        if(currentGridUnlock < TiersUnlockGrid.Length)
        {
            for(int i = 0;i < TiersUnlockGrid[currentGridUnlock].Slots.Count;i++)
            {
                TiersUnlockGrid[currentGridUnlock].Slots[i].gameObject.SetActive(true);
                gridSlots.Add(TiersUnlockGrid[currentGridUnlock].Slots[i]);
            }

        }
        ActualiseBoard();
    }

}
public class ContextAutour
{
    public List<BoardPiece> voisins = new List<BoardPiece>();
    public int nbrCaseOccupe = 0;
    public int nbrCaseLibre = 0;
}

[Serializable]
public class TierUnlockGrid
{
    public List<GridSlot> Slots;
}


