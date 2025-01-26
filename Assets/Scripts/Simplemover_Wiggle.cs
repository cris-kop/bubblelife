using UnityEngine;

public class Simplemover_Wiggle : SimpleMover_Base
{
    public float frequency = 2f;
    public float radius = 0.5f;

    private float swayProgressOffset;

    private void Start()
    {
        swayProgressOffset = Random.Range(0, Mathf.PI);
    }

    protected override void Move()
    {
        var elapsedTime = Time.time - _startTime;
        var distance = elapsedTime * speed;

        var perpendicularDir = Vector3.Cross(Vector3.up, _dir).normalized;

        var swayAmount = Mathf.Sin(swayProgressOffset + frequency * Mathf.PI * distance) * radius;

        transform.position = _startPos + _dir * distance;
        transform.position += perpendicularDir * swayAmount;
    }
}
