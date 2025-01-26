using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("setupValues")]
    public float startBubblePower = 1f;
    public float BubblePowerToVisualRatio = 4f;

    [Header("references")]
    public MeshRenderer bubbleModel;

    public GameController gameController;

    public ParticleSystem[] burstParticles;

    private int health;
    private float currentBubblePower;
    public float bubbleIncreaser = 0.1f;

    public Material bubbleMatStable;
    public Material bubbleMatWobble;
    public Material bubbleMatUnstable;
    public Material bubbleMatCritical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBubblePower = startBubblePower;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.IsGameActive() == false)
            return;

        UpdateVisuals();
        CollisionDetection(currentBubblePower);
    }

    private void CollisionDetection(float size)
    {
        var hitColliders = Physics.OverlapSphere(transform.position, size);

        foreach (var collider in hitColliders)
        {
            //Debug.Log("hit at " + collider.transform.position);
            currentBubblePower += bubbleIncreaser;
            gameController.PickupCollected(collider.gameObject, currentBubblePower);
            collider.gameObject.SetActive(false);
        }
    }

    private void UpdateVisuals()
    {
        bubbleModel.transform.localScale = Vector3.one * currentBubblePower * BubblePowerToVisualRatio;
    }

    public void PopBubble()
    {
        FindFirstObjectByType<AudioManager>().PlayPop();
    }

    public void Reset()
    {
        currentBubblePower = startBubblePower;
        SwitchBubbleState();
        UpdateVisuals();
    }

    public void SetBubbleLevel(float level)
	{
        if(level < 0.333f)
		{
            var progress = Mathf.Clamp01(level / 0.333f);
            bubbleModel.material.Lerp(bubbleMatStable, bubbleMatWobble, progress);
		}
        else if (level < 0.666f)
        {
            var progress = Mathf.Clamp01((level - 0.333f) / 0.333f);
            bubbleModel.material.Lerp(bubbleMatWobble, bubbleMatUnstable, progress);
        }
        else
        {
            var progress = Mathf.Clamp01((level - 0.666f) / 0.333f);
            bubbleModel.material.Lerp(bubbleMatUnstable, bubbleMatCritical, progress);
        }
    }

    private void SwitchBubbleState()
    {
        if (gameController.GetCurrentWorld() == GameController.worldMode.light)
        {
            bubbleModel.gameObject.SetActive(true);
        }
        else
        {
            bubbleModel.gameObject.SetActive(false);

            foreach(var p in burstParticles)
			{
                p.Play();
			}
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var originalMatrix = Gizmos.matrix;

        Gizmos.matrix = transform.localToWorldMatrix;
        var pos = Vector3.zero;

        Gizmos.DrawWireSphere(pos, currentBubblePower);

        Gizmos.matrix = originalMatrix;
    }

}
