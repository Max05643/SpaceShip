
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AsteroidsController : MonoBehaviour
{
    [System.Serializable]

    public class Settings
    {
        public bool enabled = true;

        public float spawnNoise = 50, initialSpawnDistance = 50, minSpawnDistance = 300, maxSpawnDistance = 1000, maxSpawnZ = 1900;

        public int targetCount = 1000;
    }

    [Inject]
    AsteroidController.Factory asteroidFactory;

    [SerializeField]
    Transform playerTransform;

    List<GameObject> currentAsteroids = new List<GameObject>();
    List<GameObject> unusedAsteroids = new List<GameObject>();


    [Inject]
    Settings settings;

    void Start()
    {
        if (!settings.enabled)
        {
            return;
        }

        SpawnNewAsteroids(settings.initialSpawnDistance);
        StartCoroutine(nameof(SpawnCycle));
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            RemoveOldAsteroids();
            SpawnNewAsteroids(settings.minSpawnDistance);
            yield return new WaitForSeconds(2);
        }
    }

    void RemoveOldAsteroids()
    {
        var objsToRemove = new List<GameObject>();
        foreach (GameObject asteroid in currentAsteroids)
        {
            if (asteroid.transform.position.z < playerTransform.position.z - 10)
            {
                objsToRemove.Add(asteroid);
            }
        }

        foreach (GameObject asteroid in objsToRemove)
        {
            RemoveAsteroid(asteroid);
        }
    }

    void SpawnNewAsteroids(float minSpawnDistance)
    {
        int skippedCount = 0;

        while (currentAsteroids.Count + skippedCount < settings.targetCount)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-settings.spawnNoise, settings.spawnNoise), Random.Range(-settings.spawnNoise, settings.spawnNoise), Random.Range(minSpawnDistance, settings.maxSpawnDistance) + playerTransform.position.z);

            if (spawnPosition.z > settings.maxSpawnZ)
            {
                skippedCount++;
                continue;
            }

            GameObject asteroid = GetNewAsteroid();
            asteroid.transform.position = spawnPosition;
            currentAsteroids.Add(asteroid);
        }
    }

    GameObject GetNewAsteroid()
    {
        if (unusedAsteroids.Count > 0)
        {
            GameObject asteroid = unusedAsteroids[0];
            unusedAsteroids.RemoveAt(0);
            asteroid.SetActive(true);
            return asteroid;
        }
        else
        {
            var asteroidController = asteroidFactory.Create();
            asteroidController.transform.SetParent(transform);
            GameObject asteroid = asteroidController.gameObject;
            return asteroid;
        }
    }

    void RemoveAsteroid(GameObject asteroid)
    {
        currentAsteroids.Remove(asteroid);
        asteroid.SetActive(false);
        unusedAsteroids.Add(asteroid);
    }

}
