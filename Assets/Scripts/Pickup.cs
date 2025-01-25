using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameController gameController;

    // needs to be more 'fun', not static, weave or something?
    private float moveSpeed = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        gameController = GameObject.FindFirstObjectByType<GameController>();
        //gameController.OnWorldModeChanged += worldHasChanged;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.z -= Time.deltaTime * (moveSpeed * gameController.GetCurrentLevel());

        if(newPos.z < gameController.levelZmin)
        {
            gameController.spawnMachine.DeleteObject(this.gameObject);
        }
        else
        {
            transform.position = newPos;
        }       
    }
}
