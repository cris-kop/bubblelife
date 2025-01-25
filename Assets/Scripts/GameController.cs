using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum worldMode
    {
        light, dark
    };

    public Button _startGameButton;
    public GameObject player;
    public Vector3 playerStartPosition = new Vector3(0.0f, 1.0f, 0.0f);

    private worldMode currWorldMode = worldMode.light; // used by effect, make sure its starts correct
    public Action<worldMode> OnWorldModeChanged;

    private int score = 0;
    private int health = 3;
    public int startHealth = 3;
    public Text scoreText;
    public Text healthText;
    public Text timerText;

    public int startDurationInLight;
    public int durationNeededInDark;
    private float remainDurationInWorld;

    public int scorePerPickup = 50;
    public float timeAddedPerPickup = 1.0f;

    public int damagePerHit = 1;

    public float levelZmin = -10.0f;

    public Button retryButton;
    private bool gameActive = false;
    private int currLevel = 0;

    // Sound
    public AudioSource musicLightWorld;
    public AudioSource musicDarkWorld;
    public AudioSource hitLightWorldSound;
    public AudioSource hitDarkWorldSound;

    // Solve with dispatch/msg listener?
    public SpawnMachine spawnMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startGameButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            CheckPlayerDied();
            UpdateTimers();
        }
    }

    private void SwapWorldMode(bool initial)
    {
        switch (currWorldMode)
        {
            case worldMode.dark:
                currWorldMode = worldMode.light;
                remainDurationInWorld = startDurationInLight;
                musicDarkWorld.Stop();
                musicLightWorld.Play();
                break;
            case worldMode.light:
                currWorldMode = worldMode.dark;
                remainDurationInWorld = durationNeededInDark;
                musicLightWorld.Stop();
                musicDarkWorld.Play();
                break;
        }


        player.GetComponent<Player>().Reset();
        if (initial)
        {
            player.transform.position = playerStartPosition;
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

    public void PickupCollected(GameObject pickup)
    {
        spawnMachine.DeleteObject(pickup);

        switch (currWorldMode)
        {
            case worldMode.light:
                score += scorePerPickup;
                UpdateScoreText();
                remainDurationInWorld += timeAddedPerPickup;
                hitLightWorldSound.Play();
                break;
            case worldMode.dark:
                health -= damagePerHit;
                UpdateHealthText();
                hitDarkWorldSound.Play();
                break;
        }
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
        if (health <= 0)
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
        gameActive = true;
        currLevel = 1;

        retryButton.gameObject.SetActive(false);

        UpdateHealthText();
        UpdateScoreText();

        currWorldMode = worldMode.dark;
        SwapWorldMode(true);
    }

    void UpdateTimers()
    {
        remainDurationInWorld -= Time.deltaTime;

        int tDuration = (int)remainDurationInWorld;
        timerText.text = tDuration.ToString();

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
