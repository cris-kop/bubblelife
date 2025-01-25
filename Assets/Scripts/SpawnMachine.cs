using UnityEngine;
using System.Collections.Generic;

public class SpawnMachine : MonoBehaviour
{
    public GameController gameController;
    public GameObject pickupPrefab;

    public int maxObjects = 10;
    private int currObjects = 0;

    // Spawning region
    public float minZ = -0.1f;
    public float maxZ = 0.1f;

    public float minX = -0.1f;
    public float maxX = 0.1f;

    public float spawnY = 2.8f;

    // List of active objects
    List<GameObject> objectsList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CleanUp();       
        if (gameController.IsGameActive())
        {
            if (currObjects < maxObjects)
            {
                SpawnObject();
            }
        }
    }

    // Spawn a new object
    void SpawnObject()
    {
        Vector3 spawnPos = new Vector3();
        spawnPos.x = Random.Range(minX, maxX);
        spawnPos.y = spawnY;
        spawnPos.z = Random.Range(minZ, maxZ);

        objectsList.Add((GameObject)Instantiate(pickupPrefab, spawnPos, Quaternion.identity));
        currObjects++;
    }

    // Reset the spawner
    public void Reset()
    {
        currObjects = 0;

        foreach(var element in objectsList)
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
        currObjects--;
    }

    private void CleanUp()
    {
        foreach (var element in objectsList)
        {
            if (element)
            {
                if (element.GetComponent<Transform>().position.z < gameController.levelZmin)
                {
                    Destroy(element);
                }
            }
        }
    }

    }
