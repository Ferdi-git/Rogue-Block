using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] Transform[] spotChoice;
    [SerializeField] SOEventGridManager eventGridManager;
    [SerializeField] List<SoPieces> difPieces;

    public List<GameObject> lastGeneratedPiece = new List<GameObject>();
    public ShopManager shopManager;

    private bool _isGenerating = false; 

    private void OnEnable()
    {
        eventGridManager.PiecePlaced += CheckIfPiecePlaced;
    }
    private void OnDisable()
    {
        eventGridManager.PiecePlaced -= CheckIfPiecePlaced;
    }

    [Button]
    public void GeneratePiece()
    {
        if (_isGenerating) return;
        _isGenerating = true;

        ClearChoice();

        List<SoPieces> pool = new List<SoPieces>(difPieces);

        for (int i = 0; i < spotChoice.Length; i++)
        {
            if (pool.Count == 0) break;

            int randInt = Random.Range(0, pool.Count);
            SoPieces chosen = pool[randInt];
            pool.RemoveAt(randInt);

            GameObject newPiece = Instantiate(
                chosen.prefab,
                spotChoice[i].transform.position,
                spotChoice[i].transform.rotation,
                transform
            );

            PieceInfo pieceInfo = newPiece.GetComponent<PieceInfo>();
            pieceInfo.soPiece.TempEffectValues = new List<int>(pieceInfo.soPiece.BaseEffectValues);
            lastGeneratedPiece.Add(newPiece);
        }

        _isGenerating = false;
    }

    private void ClearChoice()
    {
        for (int i = 0; i < lastGeneratedPiece.Count; i++)
        {
            if (lastGeneratedPiece[i] != null)
                Destroy(lastGeneratedPiece[i]);
        }
        lastGeneratedPiece.Clear();
    }

    private void CheckIfPiecePlaced(GameObject go)
    {
        if (lastGeneratedPiece == null || lastGeneratedPiece.Count == 0) return;

        for (int i = 0; i < lastGeneratedPiece.Count; i++)
        {
            if (go == lastGeneratedPiece[i])
            {
                lastGeneratedPiece.RemoveAt(i);
                difPieces.Remove(go.GetComponent<PieceInfo>().soPiece);
                go.transform.SetParent(null);
                eventGridManager.InvokeAddBoardPiece(go);
                eventGridManager.InvokeTrySaveInventory();
                EndChoice();
                return; 
            }
        }
    }

    public void EndChoice()
    {
        ClearChoice();
        shopManager.CloseShop();
    }
}