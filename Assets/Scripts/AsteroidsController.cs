using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsController : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidPrefab;

    [SerializeField]
    Transform playerTransform;


    [SerializeField]
    float spawnNoise = 50, initialSpawnDistance = 50, minSpawnDistance = 300, maxSpawnDistance = 1000;

    [SerializeField]
    int targetCount = 1000;



    List<GameObject> currentAsteroids = new List<GameObject>();
    List<GameObject> unusedAsteroids = new List<GameObject>();


    void Start()
    {
        SpawnNewAsteroids(initialSpawnDistance);
        StartCoroutine(nameof(SpawnCycle));
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            RemoveOldAsteroids();
            SpawnNewAsteroids(minSpawnDistance);
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
        while (currentAsteroids.Count < targetCount)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnNoise, spawnNoise), Random.Range(-spawnNoise, spawnNoise), Random.Range(minSpawnDistance, maxSpawnDistance) + playerTransform.position.z);
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
            GameObject asteroid = Instantiate(asteroidPrefab, parent: transform);
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
