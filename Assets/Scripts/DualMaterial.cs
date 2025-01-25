
using UnityEngine;
using System;

public class DualMaterial : DualWorldObject
{
    public Material materialLight;
    public Material materialDark;


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

        switch(currentWorldMode)
        {
            case GameController.worldMode.light:
                this.GetComponent<MeshRenderer>().material = materialLight;
                break;
            case GameController.worldMode.dark:
                this.GetComponent<MeshRenderer>().material = materialDark;
                break;
        }
    }
}
