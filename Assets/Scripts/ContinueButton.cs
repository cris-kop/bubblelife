using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public void OnContinueButtonClicked()
    {
        SceneManager.LoadScene("homescreen");
    }
}
