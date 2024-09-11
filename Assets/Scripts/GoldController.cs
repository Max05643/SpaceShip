using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    [SerializeField]
    GameObject circle;

    Tween currentAnimation = null;

    void OnEnable()
    {
        currentAnimation?.Kill();

        currentAnimation = circle.transform.DORotate(Vector3.forward * 180, 1).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }


    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

}
