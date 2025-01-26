using UnityEngine;
using UnityEngine.UI;

public class HighscoreInput : MonoBehaviour
{
    public Highscores highscoreManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInitialsSubmitted()
    {
        highscoreManager.PassInitials(this.GetComponent<InputField>().text);
        Debug.Log("Initials submitted!");

        this.gameObject.SetActive(false);
    }
}
