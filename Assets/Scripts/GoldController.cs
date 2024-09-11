using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    [SerializeField]
    GameObject circle;

    public GoldSpawnController goldSpawnController;

    Tween currentAnimation = null;

    public bool IsBeingGrabbed => isBeingGrabbed;

    bool isBeingDetected = false;
    bool isBeingGrabbed = false;

    public void FinishGrabbing()
    {
        goldSpawnController.RemoveGold(gameObject);
    }

    public void StartGrabbing()
    {
        isBeingGrabbed = true;
        UpdateCircle();
    }

    public void StartDetecting()
    {
        isBeingDetected = true;
        UpdateCircle();
    }

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
