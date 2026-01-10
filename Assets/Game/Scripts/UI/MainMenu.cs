using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject selectCharacterAndStageMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        mainMenu.SetActive(false);
        selectCharacterAndStageMenu.SetActive(true);
    }
    public void OptionsButtonClicked()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ControlsButtonClicked()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
    public void ExitButtonClicked()
    {
        Application.Quit();
    }
    public void BackButtonClicked()
    {
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void SelectCharactersClicked(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void SelectStageClicked(string sceneName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
