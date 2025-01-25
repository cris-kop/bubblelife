using UnityEngine;
using System;

public class DualGameobject : DualWorldObject
{
    public GameObject light;
    public GameObject dark;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void worldHasChanged(GameController.worldMode currentWorldMode)
    {
        base.worldHasChanged(currentWorldMode);

        switch (currentWorldMode)
        {
            case GameController.worldMode.light:
                light.SetActive(true);
                dark.SetActive(false);
                break;
            case GameController.worldMode.dark:
                light.SetActive(false);
                dark.SetActive(true);
                break;
        }
    }
}
