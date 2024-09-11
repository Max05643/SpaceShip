using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

/// <summary>
/// Controls the gold item
/// </summary>
public class GoldController : MonoBehaviour
{

    public class Factory : PlaceholderFactory<GoldController>
    {
    }

    [SerializeField]
    GameObject circle;

    [Inject]
    GoldSpawnController goldSpawnController;

    Tween currentAnimation = null;

    /// <summary>
    /// Is grabbing animation playing
    /// </summary>
    public bool IsBeingGrabbed => isBeingGrabbed;

    bool isBeingDetected = false;
    bool isBeingGrabbed = false;


    /// <summary>
    /// Fimish the grabbing animation and removes the gold item
    /// </summary>
    public void FinishGrabbing()
    {
        goldSpawnController.RemoveGold(gameObject);
    }

    /// <summary>
    /// Starts the grabbing animation
    /// </summary>
    public void StartGrabbing()
    {
        isBeingGrabbed = true;
        UpdateCircle();
    }

    /// <summary>
    /// Starts the detection visual effect
    /// </summary>
    public void StartDetecting()
    {
        isBeingDetected = true;
        UpdateCircle();
    }

    /// <summary>
    /// Stops the detection visual effect
    /// </summary>
    public void StopDetecting()
    {
        isBeingDetected = false;
        UpdateCircle();
    }

    void OnEnable()
    {
        isBeingDetected = false;
        isBeingGrabbed = false;

        currentAnimation?.Kill();

        currentAnimation = circle.transform.DORotate(Vector3.forward * 180, 1).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);

        UpdateCircle();
    }


    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

    void UpdateCircle()
    {
        circle.SetActive(isBeingDetected && !isBeingGrabbed);
    }

}
