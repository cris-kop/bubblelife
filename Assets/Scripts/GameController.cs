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
    public int startDurationInLight;
    public int durationNeededInDark;
    private float remainDurationInWorld;
      
    public int pickupsNeeded = 3;
    public int scorePerPickup = 50;
    public float timeAddedPerPickup = 1.0f;
    private int pickupCount = 0;

    public int damagePerHit = 1;

    public Button retryButton;
    private bool gameActive = false;
    private int currLevel = 0;

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
        switch(currWorldMode)
        {
            case (worldMode.dark):
                currWorldMode = worldMode.light;
                break;
            case (worldMode.light):
                currWorldMode = worldMode.dark;
                break;
        }
        player.GetComponent<Player>().SwitchBubbleState();
        pickupCount = 0;

        OnWorldModeChanged.Invoke(currWorldMode);

        ++currLevel;
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
        remainDurationInWorld += timeAddedPerPickup;
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
        currLevel = 1;

        worldStartTime = Time.time;
        remainDurationInWorld = startDurationInLight;
    }

    void UpdateTimers()
    {
        int currTime = (int)Time.time;
        remainDurationInWorld -= Time.deltaTime;

        int tDuration = (int)remainDurationInWorld;
        timerText.text = tDuration.ToString();

        if(remainDurationInWorld <= 0)
        {
            SwapWorldMode();
            player.GetComponent<Player>().Reset();
            player.transform.position = playerStartPosition;

            worldStartTime = currTime;

            switch (currWorldMode)
            {
                case (worldMode.dark):
                    remainDurationInWorld = durationNeededInDark;
                    break;
                case (worldMode.light):
                    remainDurationInWorld = startDurationInLight;
                    break;
            }
        }
    }

    public int GetCurrentLevel()
    {
        return currLevel;
    }
}
