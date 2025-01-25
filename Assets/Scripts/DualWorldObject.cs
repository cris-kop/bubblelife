using UnityEngine;
using System;

public class DualWorldObject : MonoBehaviour
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
        // base function, inherited class function per 'type'
    }
}
