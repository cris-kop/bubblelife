using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButton("StartGame"))
        {
            OnPlayButtonClick();
        }
    }
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Quit now!");
    }
}
