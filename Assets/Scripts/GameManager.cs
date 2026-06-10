using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOEventStartTurn turnEvent;
    [SerializeField] private SOEventGridManager gridManager;
    [SerializeField] private SOEventState gameState;
    [SerializeField] private SOEventFloor floorEvent;
    [SerializeField] private FloorListSo floorListSo;
    [SerializeField] private SOEventPlayerLostMessage lostMessage;
    [SerializeField] private SOEventPLayerWinMessage winMessage;


    public int ActualFloorCount;
    public FloorEvent ActualEvent;
    private void OnEnable()
    {
        gameState.EndOfCombat += CombateEnded;
        gameState.EndOfShoping += ShopingEnded;
        gameState.LooseEvent += PlayerLost;
        gameState.EndOfBossCombat += BossCombatEnded;
    }
    private void OnDisable()
    {
        gameState.EndOfCombat -= CombateEnded;
        gameState.EndOfShoping -= ShopingEnded;
        gameState.LooseEvent -= PlayerLost;
        gameState.EndOfBossCombat -= BossCombatEnded;

    }
    public void ButtonPressed ()
    {
        if (ActualEvent == FloorEvent.NormalFight || ActualEvent == FloorEvent.BossFight)
        {
            turnEvent.InvokeNextTurn();
        }
    }

    private void Start()
    {
        floorEvent.InvokeFirstFloor();
        ActualFloorCount = 0;
        FirstFloor();
        
    }
    private void FirstFloor ()
    {
        ActualEvent = floorListSo.list[0];
        StartEvent(ActualEvent);
    }
    private void NextFloor ()
    {
        ActualFloorCount++;
        ActualEvent = floorListSo.list[ActualFloorCount];
        floorEvent.InvokeNextFloor();
        StartEvent(ActualEvent);
    }

    private void CombateEnded ()
    {
        print("combat ended");
        gridManager.InvokeResetInventory();
        NextFloor();
    }
    private void ShopingEnded ()
    {
        print("shop ended");
        if (ActualFloorCount == 1)
        {
            gridManager.InvokeUnlockNextGridTier();
        }
        else if (ActualFloorCount == 5)
        {
            gridManager.InvokeUnlockNextGridTier();
        }
        else if (ActualFloorCount == 9)
        {
            gridManager.InvokeUnlockNextGridTier();
        }
        else if (ActualFloorCount == 13)
        {
            gridManager.InvokeUnlockNextGridTier();
        }
        else if (ActualFloorCount == 17)
        {
            gridManager.InvokeUnlockNextGridTier();
        }
        else if (ActualFloorCount == 21)
        {
            gridManager.InvokeUnlockNextGridTier();
        }

        NextFloor();
    }
    private void PlayerLost ()
    {
        gridManager.InvokeSetAllPieceCanMove(false);
        lostMessage.InvokeActiveLostMessage();
    }


    private void StartEvent (FloorEvent floorEvent)
    {
        if (floorEvent == FloorEvent.NormalFight)
        {
            gameState.InvokeStartCombat();
        }
        else if (floorEvent == FloorEvent.Shop)
        {
            gameState.InvokeStartShoping();
        }
        else if (floorEvent == FloorEvent.BossFight)
        {
            gameState.InvokeStartBossCombat();
        }
    }


    private void BossCombatEnded (int bossLevel)
    {
        if (bossLevel >= 3)
        {
            winMessage.InvokeWinMessageEvent();
        }
    }
}
