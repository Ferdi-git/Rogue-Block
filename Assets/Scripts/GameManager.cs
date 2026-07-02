using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStateManager _gameStateManager;
    private void Awake()
    {
        _gameStateManager.StartFirstState();

    }


}
