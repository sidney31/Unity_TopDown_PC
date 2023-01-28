using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
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
