using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void SelectStage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
