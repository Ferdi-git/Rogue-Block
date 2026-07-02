using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //early game states va définir les premiers etats du jeux avant le choix sur la roadmap
    //(pour la mise en place d'un tuto)
    public GameState[] earlyGameStates ;
    private int earlyGameStateIndex; 



    public void StartFirstState () // initialise l'Index 
    {
        earlyGameStateIndex = 0;
        NextState ();
    }
    public void NextState () 
    {
        if (earlyGameStateIndex < earlyGameStates.Length) // avant road map 
        {
            StartState(earlyGameStates[earlyGameStateIndex]);
            earlyGameStateIndex++;
        }
        else // avec road map 
        {
            //Open Road map et elle me renvoie le state choisi 
            //StartState(ce que m'envoie la road map)
        }
    }

    private void StartState( GameState state)
    {
        switch (state)
        {
            case GameState.FirstFight:
                //Ce que ça fait  
                break;

            case GameState.Fight:
                //Ce que ça fait  
                break;
            case GameState.Shop:
                //Ce que ça fait  
                break;
            case GameState.Campfire:
                //Ce que ça fait  
                break;
        }
    }

}

public enum GameState
{
    FirstFight,//premier combat avant roadmap
    Fight, 
    Shop,
    Campfire,
    ZoneUpgrade
}
