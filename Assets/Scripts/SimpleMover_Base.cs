using UnityEngine;

public class SimpleMover_Base : MonoBehaviour
{
    public float minSpeed = 2.0f;
    public float maxSpeed = 4.0f;

    protected float speed;
    protected Vector3 _dir;
    protected Vector3 _startPos;
    protected float _startTime;

    protected GameController gameController;



    public void Setup(Vector3 spawnPos, Vector3 dir)
    {
        _dir = dir.normalized;
        _startPos = spawnPos;
        _startTime = Time.time;

        // push random to the extremes
        var random = Random.value;
        random = random * 2 - 1;
        random = Mathf.Sign(random) * Mathf.Pow(random, 3.0f);
        random = (random + 1) * 0.5f;

        speed = Mathf.Lerp(minSpeed, maxSpeed, random);

        gameController = GameObject.FindFirstObjectByType<GameController>();

    }

    private void Update()
    {
        if (gameController.IsGameActive())
        {
            Move();
        }
    }

    protected virtual void Move()
	{

	}
}
