using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Dialogue");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StageOne()
    {
        SceneManager.LoadScene("StageOne");

    }
}
