using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float accel = 15f;
    public float drag = 7.0f;


    private Vector3 velocity;

    public GameController gameController;

    private Vector3 topRight;
    private Vector3 bottomLeft;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        Debug.Log(topRight);
        Debug.Log(bottomLeft);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.IsGameActive())
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
            float mag = velocity.magnitude;
            float dragStrength = mag / moveSpeed;
            dragStrength = Mathf.Pow(dragStrength, 1 / 3.0f);

            mag = Mathf.MoveTowards(mag, moveSpeed * 0.08f, drag * Time.deltaTime * dragStrength);
            velocity = velocity.normalized * mag;

            velocity += movement * accel * Time.deltaTime;
            if (velocity.magnitude > moveSpeed)
            {
                velocity = velocity.normalized * moveSpeed;
            }

            transform.position += velocity * Time.deltaTime;

            if (transform.position.x < topRight.x)
            {
                transform.position = new Vector3(topRight.x, transform.position.y, transform.position.z);
            }

            if (transform.position.x > bottomLeft.x)
            {
                transform.position = new Vector3(bottomLeft.x, transform.position.y, transform.position.z);
            }

            if (transform.position.z > topRight.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, topRight.z);
            }

            if (transform.position.z < bottomLeft.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, bottomLeft.z);
            }
        }
    }
}
