using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Zenject;

public class GoldSpawnController : MonoBehaviour
{

    [System.Serializable]
    public class Settings
    {
        public bool enabled = true;
        public float spawnNoise = 50, initialSpawnDistance = 50, minSpawnDistance = 300, maxSpawnDistance = 1000;
        public int targetCount = 1000;

    }

    [Inject]
    GoldController.Factory goldFactory;

    [SerializeField]
    Transform playerTransform;

    public IEnumerable<GameObject> CurrentGolds => currentgolds;

    List<GameObject> currentgolds = new List<GameObject>();
    List<GameObject> unusedgolds = new List<GameObject>();

    [Inject]
    Settings settings;


    void Start()
    {
        if (!settings.enabled)
        {
            return;
        }

        SpawnNewGolds(settings.initialSpawnDistance);
        StartCoroutine(nameof(SpawnCycle));
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            RemoveOldGolds();
            SpawnNewGolds(settings.minSpawnDistance);
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
        while (currentgolds.Count < settings.targetCount)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-settings.spawnNoise, settings.spawnNoise), Random.Range(-settings.spawnNoise, settings.spawnNoise), Random.Range(minSpawnDistance, settings.maxSpawnDistance) + playerTransform.position.z);
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
            var coldController = goldFactory.Create();
            coldController.transform.SetParent(transform);
            GameObject gold = coldController.gameObject;
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
