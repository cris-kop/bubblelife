using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum worldMode
    {
        light, dark
    };

    public GameObject player;
    public Vector3 playerStartPosition = new Vector3(0.0f, 1.0f, 0.0f);

    private worldMode currWorldMode;
    public Action<worldMode> OnWorldModeChanged;

    private bool isFirstUpdate = true;

    private int score = 0;
    private int health = 3;
    public int startHealth = 3;
    public Text scoreText;
    public Text healthText;
    public Text timerText;

    private float worldStartTime = 0;
    public int durationInLight;
    public int durationInDark;

    public int pickupsNeeded = 3;
    public int scorePerPickup = 50;
    private int pickupCount = 0;

    public int damagePerHit = 1;

    public Button retryButton;
    private bool gameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            currWorldMode = worldMode.dark;
            SwapWorldMode();
        }

        if (gameActive)
        {
            // Check if the world needs switching?
            if (pickupCount == pickupsNeeded)
            {
                player.transform.position = playerStartPosition;
                SwapWorldMode();
            }

            CheckPlayerDied();
            UpdateTimers();
        }
    }

    public void SwapWorldMode()
    {
        if(currWorldMode == worldMode.dark)
        {
            currWorldMode = worldMode.light;
        }
        else
        {
            currWorldMode = worldMode.dark;
        }
        player.GetComponent<Player>().SwitchBubbleState();
        pickupCount = 0;

        OnWorldModeChanged.Invoke(currWorldMode);
    }
    public worldMode GetCurrentWorld()
    {
        return currWorldMode;
    }

    public void PickupCollected()
    {
        pickupCount++;
        score = score + scorePerPickup;
        UpdateScoreText();
    }

    public void TakeDamage()
    {
        health = health - damagePerHit;
        UpdateHealthText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    public void UpdateHealthText()
    {
        healthText.text = "Health: " + health;
    }

    private void CheckPlayerDied()
    {
        if(health <= 0)
        {
            gameActive = false;
            retryButton.gameObject.SetActive(true);
        }
    }
    public bool IsGameActive()
    {
        return gameActive;
    }

    public void StartGame()
    {
        score = 0;
        health = startHealth;
        currWorldMode = worldMode.dark;
        SwapWorldMode();

        retryButton.gameObject.SetActive(false);
        UpdateHealthText();
        UpdateScoreText();
        player.GetComponent<Player>().Reset();
        player.transform.position = playerStartPosition;

        gameActive = true;

        worldStartTime = Time.time;
    }

    void UpdateTimers()
    { 
        float currTime = Time.time;
        float timeToCheck = 0.0f;

        int timerTextVal = (int)(currTime - worldStartTime);

        if (currWorldMode == worldMode.light)
        {
            timeToCheck = durationInLight;
            timerTextVal = durationInLight - timerTextVal;
        }
        else
        {
            timeToCheck = durationInDark;
            timerTextVal = durationInDark - timerTextVal;
        }
        timerText.text = timerTextVal.ToString();

        if (currTime - worldStartTime > timeToCheck)
        {
            SwapWorldMode();
            player.GetComponent<Player>().Reset();
            player.transform.position = playerStartPosition;

            worldStartTime = currTime;
        }
    }

}
