using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private int MenuSceneNumber;
    [SerializeField] private int GameSceneNumber;

    private void Start()
    {
        MenuSceneNumber = SceneManager.GetSceneByName("Menu").buildIndex;
        GameSceneNumber = SceneManager.GetSceneByName("Game").buildIndex;    
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void CloseGame()
    {
        Application.Quit();
    }

    public void InMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
