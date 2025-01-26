using UnityEngine;
using System.Collections.Generic;

public class SpawnMachine : MonoBehaviour
{
    public GameController gameController;
    public GameObject pickupPrefab;

    public int maxObjectsBase = 10;
    // Spawning region
    public float minZ = -0.1f;
    public float maxZ = 0.1f;

    public float minX = -0.1f;
    public float maxX = 0.1f;

    public float spawnY = 2.8f;
    
    // List of active objects
    List<GameObject> objectsList = new List<GameObject>();

    private Vector3 topRight;
    private Vector3 topLeft;

    private float nextSpawnTime = 0;
    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CleanUpOutsideScreen();

        if (gameController.IsGameActive())
        {
            if (Time.time > nextSpawnTime && objectsList.Count < (maxObjectsBase * gameController.GetCurrentLevel() / 2))
            {
                SpawnObject();
            }
        }
    }

    // Spawn a new object
    void SpawnObject()
    {
        Vector3 spawnPos = new Vector3();
        spawnPos.x = Random.Range(topRight.x, topLeft.x);
        spawnPos.y = spawnY;
        spawnPos.z = Random.Range(topRight.z + 1.0f, topRight.z + 3.0f);

        var dir = Vector3.back;
        var diToPlayer = player.transform.position - spawnPos;

        dir = Vector3.Lerp(dir, diToPlayer, Random.Range(0.5f, 0.98f));
        dir.y = 0;
        dir = dir.normalized;

        var pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        var pickupMovers = pickup.GetComponents<SimpleMover_Base>();
        foreach (var mover in pickupMovers)
        {
            mover.Setup(spawnPos, dir);
        }

        objectsList.Add(pickup);
        nextSpawnTime = Time.time + Random.Range(0.5f, 1.0f);
    }

    // Reset the spawner
    public void DeleteAll()
    {
        foreach (var element in objectsList)
        {
            if (element)
            {
                Destroy(element);
            }
        }

        objectsList.Clear();
    }

    public void DeleteObject(GameObject objectToDelete)
    {
        objectsList.Remove(objectToDelete);
        Destroy(objectToDelete);
    }

    private void CleanUpOutsideScreen()
    {
        for (int i = objectsList.Count - 1; i > -1; i--) // go through in reverse order so tht removeing the element from the list while iterating over it causes no issues, the i remains valid for each step
        {
            var element = objectsList[i];
            if (element)
            {
                if (element.transform.position.z < gameController.levelZmin)
                {
                    DeleteObject(element);
                }
            }
        }
    }

}
