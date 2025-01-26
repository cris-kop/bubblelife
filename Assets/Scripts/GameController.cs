using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum worldMode
    {
        light, dark
    };

    public GameObject _mainMenu;
    public Button _startGameButton;
    [Space]
    public GameObject _gameOverMenu;
    public GameObject _gameplayMenu;

    public Player player;
    public Vector3 playerStartPosition = new Vector3(0.0f, 1.0f, 0.0f);

    private worldMode currWorldMode = worldMode.light; // used by effect, make sure its starts correct
    public Action<worldMode> OnWorldModeChanged;

    private int score = 0;
    private int health = 3;
    public int startHealth = 3;
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text bubbleOverFlowText;
    public TMP_Text timerText;

    public int startDurationInLight;
    public int durationNeededInDark;
    private float remainDurationInWorld;
    
    public float maxBubblePower = 2.0f;
    private float currBubbleOverflow = 0.0f;
    public float decreaseOverflow = 0.05f;
    public float increaseOverflow = 0.2f;

    private float lastPickup;
    private int pickupsCollected = 0;

    public int scorePerPickup = 50;
    public float timeAddedPerPickup = 1.0f;

    public int damagePerHit = 1;

    public float levelZmin = -10.0f;

    public Button retryButton;
    private bool gameActive = false;
    private int currLevel = 0;

    // Solve with dispatch/msg listener?
    public SpawnMachine spawnMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_mainMenu.SetActive(true);
        //_gameOverMenu.SetActive(false);
        //_gameplayMenu.SetActive(false);

        //_startGameButton.onClick.AddListener(StartGame);

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            CheckPlayerDied();
            UpdateTimers();

            if(currWorldMode == worldMode.light)
            {
                currBubbleOverflow -= decreaseOverflow * Time.deltaTime;
                //Mathf.Clamp01(currBubbleOverflow);
                currBubbleOverflow = Mathf.Clamp01(currBubbleOverflow);

                player.SetBubbleLevel(currBubbleOverflow);

                UpdateBubbleOverflowText();
            }
        }
    }

    private void EnableGame()
	{
        gameActive = true;
    }

    private void SwapWorldMode(bool initial)
    {
        if(initial == false)
		{
            // pause / animate then do everything
            gameActive = false;

            // after X seconds gameActive = true;
            Invoke(nameof(EnableGame), 0.7f);
        }

        switch (currWorldMode)
        {
            case worldMode.dark:
                currWorldMode = worldMode.light;
                remainDurationInWorld = startDurationInLight;
                bubbleOverFlowText.gameObject.SetActive(true);
                lastPickup = 0;
                currBubbleOverflow = 0.0f;
                break;
            case worldMode.light:
                currWorldMode = worldMode.dark;
                remainDurationInWorld = durationNeededInDark;
                bubbleOverFlowText.gameObject.SetActive(false);
                break;
        }
        pickupsCollected = 0;

        player.Reset();
        if (initial)
        {
            player.transform.position = playerStartPosition;
        }
        else
        {
            player.PopBubble();
        }

        //player.GetComponent<Player>().SwitchBubbleState();
        spawnMachine.DeleteAll();

        if (initial == false)
        {
            currLevel++;
        }

        OnWorldModeChanged.Invoke(currWorldMode);
    }
    public worldMode GetCurrentWorld()
    {
        return currWorldMode;
    }

    public void PickupCollected(GameObject pickup, float currBubblePower)
    {
        spawnMachine.DeleteObject(pickup);
        FindFirstObjectByType<AudioManager>().PlayHitSound();
        pickupsCollected++;

        switch (currWorldMode)
        {
            case worldMode.light:
                score += scorePerPickup;
                UpdateScoreText();
                remainDurationInWorld += timeAddedPerPickup;

                currBubbleOverflow += increaseOverflow;

                if(currBubblePower > maxBubblePower || currBubbleOverflow >= 1.0f)
                {
                    remainDurationInWorld = 0.0f;
                }
                lastPickup = Time.time;
                pickupsCollected++;

                break;
            case worldMode.dark:
                health -= damagePerHit;
                UpdateHealthText();
                break;
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score:\n" + score;
    }
    public void UpdateHealthText()
    {
        healthText.text = "Health:\n" + health;
    }

    public void UpdateBubbleOverflowText()
    {
        bubbleOverFlowText.text = "Overflow:\n" + (int)(currBubbleOverflow * 100) + "%";
    }

    private void CheckPlayerDied()
    {
        if (health <= 0)
        {
            gameActive = false;
            SceneManager.LoadScene("highscores");
        }
    }
    public bool IsGameActive()
    {
        return gameActive;
    }

    public void StartGame()
    {
        _mainMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
        _gameplayMenu.SetActive(true);

        score = 0;
        health = startHealth;
        gameActive = true;
        currLevel = 1;

        UpdateHealthText();
        UpdateScoreText();

        currWorldMode = worldMode.dark;
        SwapWorldMode(true);

        timerText.gameObject.SetActive(true);
    }

    void UpdateTimers()
    {
        remainDurationInWorld -= Time.deltaTime;

        int tDuration = (int)remainDurationInWorld;

        switch (currWorldMode)
        {
            case worldMode.dark:
                timerText.text = "Survive for " + tDuration.ToString() + " seconds";
                break;
            case worldMode.light:
                timerText.text = "Chilling in my bubble for " + tDuration.ToString() + " seconds";
                break;
        }

        if (remainDurationInWorld <= 0)
        {
            SwapWorldMode(false);

            switch (currWorldMode)
            {
                case worldMode.dark:
                    remainDurationInWorld = durationNeededInDark;
                    break;
                case worldMode.light:
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
