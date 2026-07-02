using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private RSE_StateEnded _stateEndedEvent;
    private void Awake()
    {
        _stateEndedEvent.stateEndedEvent += AskNextState;
    }
    private void Start()
    {
        _gameStateManager.StartFirstState();
    }

    private void AskNextState ()
    {
        _gameStateManager.NextState();
    }

}
