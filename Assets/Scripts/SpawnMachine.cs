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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CleanUpOutsideScreen();

        if (gameController.IsGameActive())
        {
            if (objectsList.Count < (maxObjectsBase * gameController.GetCurrentLevel() / 2))
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

        //Debug.Log(spawnPos);
        var pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        var pickupMovers = pickup.GetComponents<SimpleMover_Base>();
        foreach (var mover in pickupMovers)
        {
            mover.Setup(spawnPos, Vector3.back);
        }

        objectsList.Add(pickup);
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
