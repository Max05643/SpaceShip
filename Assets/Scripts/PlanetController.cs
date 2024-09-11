using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;


/// <summary>
/// Controls visual effects of the planet
/// </summary>
public class PlanetController : MonoBehaviour
{

    [Serializable]
    public class Settings
    {
        public float minDistanceFromPlayer = 200, maxDistanceFromPlayer = 500, minPlayersZPos = 0, maxPlayersZPos = 1000, rotationTime = 120;
    }

    [SerializeField]
    Transform playerTransform;
    Tween currentAnimation;

    [Inject]
    Settings settings;


    bool isMoving = true;

    /// <summary>
    /// Planet movement is disabled, i.e. it does not follow the player anymore
    /// </summary>
    public void DisableMovement()
    {
        isMoving = false;
    }

    void Start()
    {
        currentAnimation = transform.DORotate(Vector2.up * 180, settings.rotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        var progress = Mathf.InverseLerp(settings.minPlayersZPos, settings.maxPlayersZPos, playerTransform.position.z);
        progress = Mathf.Clamp01(progress);


        var distanceFromPlayer = Mathf.Lerp(settings.maxDistanceFromPlayer, settings.minDistanceFromPlayer, progress);
        transform.position = playerTransform.position + Vector3.forward * distanceFromPlayer;
    }

    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

}
