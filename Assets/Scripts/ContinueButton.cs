using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetButtonDown("StartGame"))
        {
            OnContinueButtonClicked();
        }
    }
    public void OnContinueButtonClicked()
    {
        SceneManager.LoadScene("homescreen");
    }
}
