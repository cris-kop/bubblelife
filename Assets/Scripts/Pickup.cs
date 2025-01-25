using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameController gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        gameController = GameObject.FindFirstObjectByType<GameController>();
        gameController.OnWorldModeChanged += worldHasChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void worldHasChanged(GameController.worldMode currentWorldMode)
    {
        this.gameObject.SetActive(true);
    }
}
