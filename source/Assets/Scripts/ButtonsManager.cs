using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour, ISelectHandler
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

    public void OnSelect(BaseEventData eventData)
    {
        FindObjectOfType<AudioManager>().Play("Button");
        Debug.Log("PRESSED");
    }
}
