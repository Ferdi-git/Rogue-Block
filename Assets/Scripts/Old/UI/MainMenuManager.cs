using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject GOOptionsMenu;
    [SerializeField] GameObject GOMainMenu;

    private void Start()
    {
        GOOptionsMenu.SetActive(false);
        GOMainMenu.SetActive(true);
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions() 
    {
        GOMainMenu.SetActive(false);
        GOOptionsMenu.SetActive(true);
    }


    public void OpenMainMenu()
    {
        GOMainMenu.SetActive(true);
        GOOptionsMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadTuto()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
