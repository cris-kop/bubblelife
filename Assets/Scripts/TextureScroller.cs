using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public GameController gameController;

    public float scrollSpeedMultiplier = 0.5f;
    public float maxScrollSpeed = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newOffset = this.GetComponent<MeshRenderer>().material.mainTextureOffset;

        float multiplier = Mathf.Min(2.0f, scrollSpeedMultiplier * gameController.GetCurrentLevel());

        newOffset.y -= Time.deltaTime * multiplier;
        this.GetComponent<MeshRenderer>().material.mainTextureOffset = newOffset;

    }
}
