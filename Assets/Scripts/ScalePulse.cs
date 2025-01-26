using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    public float duration;
    public float min;
    public float max;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(min, max, Mathf.Abs(Mathf.Sin(Time.time * Mathf.PI * 2 * duration)));
    }
}
