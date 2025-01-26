using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
