using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    float distanceFromPlayer = 500;


    void Update()
    {
        transform.position = playerTransform.position + Vector3.forward * distanceFromPlayer;
    }

}
