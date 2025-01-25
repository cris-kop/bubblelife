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

    public void SwapWorldMode()
    {
        switch(currWorldMode)
        {
            case (worldMode.dark):
                currWorldMode = worldMode.light;
                musicDarkWorld.Stop();
                musicLightWorld.Play();
                break;
            case (worldMode.light):
                currWorldMode = worldMode.dark;
                musicLightWorld.Stop();
                musicDarkWorld.Play();
                break;
        }
        player.GetComponent<Player>().SwitchBubbleState();
        OnWorldModeChanged.Invoke(currWorldMode);
        spawnMachine.Reset();
        ++currLevel;
    }
    public worldMode GetCurrentWorld()
    {
        return currWorldMode;
    }

    public void PickupCollected(GameObject pObject)
    {
        score = score + scorePerPickup;
        UpdateScoreText();

        spawnMachine.DeleteObject(pObject);
        remainDurationInWorld += timeAddedPerPickup;

        switch(currWorldMode)
        {
            case (worldMode.light):
                hitLightWorldSound.Play();
                break;
            case (worldMode.dark):
                hitDarkWorldSound.Play();
                break;
        }
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

        spawnMachine.Reset();
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
