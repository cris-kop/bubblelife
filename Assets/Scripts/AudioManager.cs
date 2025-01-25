using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    // Sound
    [Header("prefabs")]
    [SerializeField] private AudioSource effectPrefab;

    [Header("clips")]
    [SerializeField] private AudioClip hitLightWorld;
    [SerializeField] private AudioClip hitDarkWorld;
    [SerializeField] private AudioClip popSFX;

    [Header("permanent")]
    [SerializeField] private AudioSource musicLight;
    [SerializeField] private AudioSource musicDark;

    private GameController gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicLight.volume = 0;
        musicDark.volume = 0;
        musicLight.Play();
        musicDark.Play();

        gameController = FindFirstObjectByType<GameController>();
        gameController.OnWorldModeChanged += OnWorldChanged;
        OnWorldChanged(gameController.GetCurrentWorld());
    }

    private void OnWorldChanged(GameController.worldMode current)
    {
        musicLight.DOKill(false);
        musicDark.DOKill(false);

        musicLight.DOFade(current == GameController.worldMode.light ? 1 : 0, 0.5f);
        musicDark.DOFade(current == GameController.worldMode.dark ? 1 : 0, 0.5f);

        switch (current)
        {
            case GameController.worldMode.light:
                musicLight.Stop();
                musicLight.Play();
                break;
            case GameController.worldMode.dark:
                musicDark.Stop();
                musicDark.Play();
                break;
        }
    }

    public void PlayHitSound()
    {
        var world = gameController.GetCurrentWorld();
        var effect = Instantiate(effectPrefab);

        switch (world)
        {
            case GameController.worldMode.light:
                effect.clip = hitLightWorld;
                break;
            case GameController.worldMode.dark:
                effect.clip = hitDarkWorld;
                break;
        }

        effect.Play();
    }

    public void PlayPop()
    {
        var effect = Instantiate(effectPrefab);
        effect.clip = popSFX;
        effect.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
