using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject resultPanel;
    public Text resultText;

    public FightingController[] fightingController;
    public OpponentAI[] opponentAI;

    private void Update()
    {
        foreach(FightingController fightingController in fightingController)
        {
            if(fightingController.gameObject.activeSelf && fightingController.currentHealth <= 0)
            {
                setResult("You Lose!");
                return;
            }
        }
        foreach(OpponentAI opponentAI in opponentAI)
        {
            if(opponentAI.gameObject.activeSelf && opponentAI.currentHealth <= 0)
            {
                setResult("You Win!");
                return;
            }
        }
    }

    void setResult(string result)
    {
        resultPanel.SetActive(true);
        resultText.text = result;
        Time.timeScale = 0f; // Pause the game
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("MainMenu");
    }
}
