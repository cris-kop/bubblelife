using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public GameController gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.IsGameActive())
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
    }
}
