using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    GameObject[] possibleMeshes;


    Sequence currentAnimation = null;

    void OnEnable()
    {
        int meshToDisplay = Random.Range(0, possibleMeshes.Length);
        for (int i = 0; i < possibleMeshes.Length; i++)
        {
            possibleMeshes[i].SetActive(i == meshToDisplay);
        }

        currentAnimation?.Kill();

        currentAnimation = DOTween.Sequence();
        var rotationTarget = Random.insideUnitSphere * 360;
        currentAnimation.Append(possibleMeshes[meshToDisplay].transform.DOLocalRotate(rotationTarget, 2f).SetEase(Ease.Linear).From(Vector3.zero));
        currentAnimation.Append(possibleMeshes[meshToDisplay].transform.DOLocalRotate(Vector3.zero, 2f).SetEase(Ease.Linear).From(rotationTarget));
        currentAnimation.SetLoops(-1, LoopType.Restart);
    }


    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

}

