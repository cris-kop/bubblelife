using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newOffset = this.GetComponent<MeshRenderer>().material.mainTextureOffset;

        newOffset.y -= Time.deltaTime * scrollSpeed;
        this.GetComponent<MeshRenderer>().material.mainTextureOffset = newOffset;

    }
}
