using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    float distanceFromPlayer = 500;

    [SerializeField]
    float rotationTime = 120;


    Tween currentAnimation;

    void Start()
    {
        currentAnimation = transform.DORotate(Vector2.up * 180, rotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
    void Update()
    {
        transform.position = playerTransform.position + Vector3.forward * distanceFromPlayer;
    }

    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

}
