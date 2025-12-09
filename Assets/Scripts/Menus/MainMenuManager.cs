using System.Net.Mime;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject menuButtons;

    public void SwapScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        menuButtons.SetActive(false);
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
        menuButtons.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
