using System;
using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private SOEventStartTurn turnEvent;


    [SerializeField] private SoBoard soBoard;
    [SerializeField] private bool skipFight;
    [SerializeField] private StatsPlayer statsPlayer;
    [SerializeField] private StatsEnnemi statsEnnemi;

    [SerializeField] private int index;
    [SerializeField] private GameObject bouton;

    [SerializeField] private SoNbrOfPiecePlayed piecePlayed;
    [SerializeField] private SOEventGridManager eventGridManager;
    [SerializeField] private PieceHealthManager pieceHealthManager;
    [SerializeField] private SOEventEnnemy eventEnnemi;
    [SerializeField] private string lastEnemyName;
    [SerializeField] private SOEventVisuelEffect eventVisuel;

    [SerializeField] private SOEventState eventState;

    [SerializeField] private SOEventEndPlayerTurn endTurn;

    private float enemyStartTime;
    private int turnsThisEnemy;

    public int NbrOfCombat;
    public int NbrOfBoss;
    private int nbrOfTurn;
    private void OnEnable()
    {
        eventState.StartCombat += StartCombat;
        eventState.StartBossCombat += StartBossCombat;
        turnEvent.NextTurn += StartTurn;
    }
    private void OnDisable()
    {
        eventState.StartCombat -= StartCombat;
        eventState.StartBossCombat -= StartBossCombat;

        turnEvent.NextTurn -= StartTurn;
    }

    private void Awake()
    {
        bouton.SetActive(true);
        piecePlayed.ResetInt();
        nbrOfTurn = 0;
        NbrOfCombat = 0;
        NbrOfBoss = 0;
        statsPlayer.InvokeStartPVSet();
    }

    public void StartBossCombat ()
    {
        nbrOfTurn = 0;
        enemyStartTime = Time.realtimeSinceStartup;
        turnsThisEnemy = 0;

        bouton.SetActive(true);
        eventEnnemi.InvokeGenerateBoss(NbrOfBoss);
        eventEnnemi.InvokeEnnemiShowAttack();
        eventEnnemi.InvokeNewEnnemi();
        eventGridManager.InvokeSetAllPieceCanMove(true);
        NbrOfBoss++;
    }
    public void StartCombat ()
    {
        nbrOfTurn = 0;
        enemyStartTime = Time.realtimeSinceStartup;
        turnsThisEnemy = 0;

        bouton.SetActive(true);
        eventEnnemi.InvokeGenerateEnnemi(NbrOfCombat);
        eventEnnemi.InvokeEnnemiShowAttack();
        eventEnnemi.InvokeNewEnnemi();
        eventGridManager.InvokeSetAllPieceCanMove(true);
    }

    public void StartTurn ( )
    {
        bouton.SetActive(true);
        nbrOfTurn++;
        turnsThisEnemy++;
        AnalyticsManager.Instance.OnTurnStart();

        eventGridManager.InvokeSetAllPieceCanMove(false);
        eventGridManager.InvokeActualiseBoard();
        piecePlayed.ResetInt();

        for ( int i = 0;i < soBoard.boardPieces.Count; i++)
        {
            soBoard.boardPieces[i].context.NbrDeRepetition = 0;
        }


        index = 0;
        if (index >= soBoard.boardPieces.Count )
        {
            if (statsEnnemi.GetPV() > 0)
            {
                StartCoroutine(EnnemiTurn());
                return;
            }
            else
            {
                StartCoroutine(ResoudreTurn());
                return;
            }
            
        }
        StartCoroutine(PlayerTurn(0));
    }

    public void NextPiece ()
    {
        index++;
        if (index >= soBoard.boardPieces.Count  )
        {
            if (statsEnnemi.GetPV() > 0)
            {
                StartCoroutine(EnnemiTurn());
                return;
            }
            else
            {
                StartCoroutine(ResoudreTurn());
                return;
            }

            
        }
        StartCoroutine(PlayerTurn(index));
    }
    
    

    IEnumerator PlayerTurn (int i)
    {
        
        yield return ResoudreEffet(soBoard.boardPieces[i].soPieces, i );
        NextPiece();
    }

    private IEnumerator ResoudreEffet ( SoPieces piece , int i)
    {
        ConditionOutput conditionOutput = new ConditionOutput();
        OutputPort port = new OutputPort();
        port.statsPlayer = statsPlayer;
        port.statsEnnemi = statsEnnemi;
        port.thisBoardPiece = soBoard.boardPieces[i];
        port.piecePlayed = piecePlayed;
        soBoard.boardPieces[i].context.Tour  = nbrOfTurn;
        conditionOutput.port = port;
        conditionOutput.context = soBoard.boardPieces[i].context;
        conditionOutput.variableList = piece.ConditionValues;
        soBoard.boardPieces[i].context.healthManager = pieceHealthManager;
        
        if (piece.pieceEffet.condition.Condition(conditionOutput))
        {
            soBoard.boardPieces[i].context.conditionOutput = conditionOutput;
            yield return piece.pieceEffet.effet.Effet(soBoard.boardPieces[i].context,port, piece.TempEffectValues , i);
            
        }
        else
        {
            yield return soBoard.boardPieces[i].pieceAnimation.PlayAnimations(i, PieceAnimations.TypeAnim.failed, null);
        }
        
    }
    IEnumerator EnnemiTurn()
    {
        int indexPieceDamaged = 0;
        int indexPieceMana = 0;
        yield return null;
        int zoneCount = statsEnnemi.actualAtkZoneNbr;
        print("Nombre de case que prend l'attque : " + zoneCount);
        for (int i = 0; i < soBoard.boardPieces.Count; i++)
        {

            if (soBoard.boardPieces[i].context.NbrCaseGenerateMana != 0)
            {
                int nbrMana = soBoard.boardPieces[i].context.NbrCaseGenerateMana * soBoard.boardPieces[i].soPieces.numberManaGeneratingPerSquare;

                bool ended = false;
                Action trailEvent = () => ended = true;
                eventVisuel.InvokeEffectGainMana(new VisuelAttakData
                {
                    nbrDMG = nbrMana,
                    posAttacker = soBoard.boardPieces[i].pieceInfo.transform.position,
                    eventEndVisuel = trailEvent
                });

                yield return soBoard.boardPieces[i].pieceAnimation.PlayAnimations(indexPieceMana, PieceAnimations.TypeAnim.generateMana, null);
                yield return new WaitUntil(() => ended);

                indexPieceMana++;
            }

            if (soBoard.boardPieces[i].context.NbrCaseAtk != 0)
            {
                print("Nombre de case d'attaque sur la piece : " + soBoard.boardPieces[i].context.NbrCaseAtk);
                yield return soBoard.boardPieces[i].pieceAnimation.PlayAnimations(indexPieceDamaged, PieceAnimations.TypeAnim.takeDamage, null);

                zoneCount -= soBoard.boardPieces[i].context.NbrCaseAtk;
                pieceHealthManager.GiveStats(soBoard.boardPieces[i].healthPoint, soBoard.boardPieces[i].shield, soBoard.boardPieces[i]);
                pieceHealthManager.TakeDamage(statsEnnemi.actualAtkDamage * soBoard.boardPieces[i].context.NbrCaseAtk);
                indexPieceDamaged++;
            }

        }

        statsPlayer.InvokeTakeDamage(statsEnnemi.actualAtkDamage * zoneCount); // degats que recoit le joueur 
                                                                                  //print ("Nombre de case qui vont touché le joueur : "+zoneCount);
                                                                                  //print("le joeur se prend " + ennemiManager.GetDamageValue() * zoneCount + " degats");

        StartCoroutine(ResoudreTurn());
    }

    IEnumerator ResoudreTurn ()
    {


        yield return StartCoroutine(RemoveAllShields());
        piecePlayed.ResetInt();
        for (int i = 0; i < soBoard.boardPieces.Count; i++)
        {
            soBoard.boardPieces[i].context.NbrDeRepetition = 0;
        }
        //check si des pieces sont mortes 
        //enlever bouclier aux pieces (mettre bouclier dans boardpiece)
        //print("test");
        eventEnnemi.InvokeEnnemiRemoveAttack();
        eventEnnemi.InvokeEnnemiShowAttack();

        if (statsPlayer.GetPV() <= 0 )
        {
            eventState.InvokeLooseEvent();
        }
        else
        {
            if (statsEnnemi.GetPV() <= 0 )
            {
                NbrOfCombat += 1;

                AnalyticsManager.Instance.OnEnemyKilled(turnsThisEnemy, Time.realtimeSinceStartup - enemyStartTime);
                AnalyticsManager.Instance.OnTurnEnd();

                bouton.SetActive(false);
                eventState.InvokeEndOfCombat();
                if (NbrOfBoss >= 3)
                {
                    eventState.InvokeEndOfBossCombat(3);
                }
            }
        }
        
        endTurn.InvokeEndTurn();
        eventGridManager.InvokeSetAllPieceCanMove(true);
    }

    IEnumerator RemoveAllShields ()
    {
        int indexRemoveShield = 0;
        for (int i = 0; i < soBoard.boardPieces.Count; i++)
        {
            if (soBoard.boardPieces[i].shield != 0)
            {
                soBoard.boardPieces[i].shield = 0;
                yield return soBoard.boardPieces[i].pieceAnimation.PlayAnimations(indexRemoveShield, PieceAnimations.TypeAnim.loseShield, null);
                indexRemoveShield += 1;
            }
        }
    }
        
}

