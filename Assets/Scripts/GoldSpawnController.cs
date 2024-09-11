using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSpawnController : MonoBehaviour
{

    [SerializeField]
    GameObject goldPrefab;

    [SerializeField]
    Transform playerTransform;


    [SerializeField]
    float spawnNoise = 50, initialSpawnDistance = 50, minSpawnDistance = 300, maxSpawnDistance = 1000;

    [SerializeField]
    int targetCount = 1000;



    List<GameObject> currentgolds = new List<GameObject>();
    List<GameObject> unusedgolds = new List<GameObject>();


    void Start()
    {
        SpawnNewGolds(initialSpawnDistance);
        StartCoroutine(nameof(SpawnCycle));
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            RemoveOldGolds();
            SpawnNewGolds(minSpawnDistance);
            yield return new WaitForSeconds(2);
        }
    }

    void RemoveOldGolds()
    {
        var objsToRemove = new List<GameObject>();
        foreach (GameObject gold in currentgolds)
        {
            if (gold.transform.position.z < playerTransform.position.z - 10)
            {
                objsToRemove.Add(gold);
            }
        }

        foreach (GameObject gold in objsToRemove)
        {
            if (gold.GetComponent<GoldController>().IsBeingGrabbed)
            {
                continue;
            }

            RemoveGold(gold);
        }
    }

    void SpawnNewGolds(float minSpawnDistance)
    {
        while (currentgolds.Count < targetCount)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnNoise, spawnNoise), Random.Range(-spawnNoise, spawnNoise), Random.Range(minSpawnDistance, maxSpawnDistance) + playerTransform.position.z);
            GameObject gold = GetNewGold();
            gold.transform.position = spawnPosition;
            currentgolds.Add(gold);
        }
    }

    GameObject GetNewGold()
    {
        if (unusedgolds.Count > 0)
        {
            GameObject gold = unusedgolds[0];
            unusedgolds.RemoveAt(0);
            gold.SetActive(true);
            return gold;
        }
        else
        {
            GameObject gold = Instantiate(goldPrefab, parent: transform);
            gold.GetComponent<GoldController>().goldSpawnController = this;
            return gold;
        }
    }

    public void RemoveGold(GameObject gold)
    {
        currentgolds.Remove(gold);
        gold.SetActive(false);
        unusedgolds.Add(gold);
    }
}
