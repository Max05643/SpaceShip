using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class AsteroidController : MonoBehaviour
{

    public class Factory : PlaceholderFactory<AsteroidController>
    {
    }

    [SerializeField]
    GameObject[] possibleMeshes;


    Tween currentAnimation = null;

    void OnEnable()
    {
        transform.rotation = Random.rotation;

        int meshToDisplay = Random.Range(0, possibleMeshes.Length);
        for (int i = 0; i < possibleMeshes.Length; i++)
        {
            possibleMeshes[i].SetActive(i == meshToDisplay);
        }

        currentAnimation?.Kill();

        var rotationTime = Random.Range(1f, 5f);

        currentAnimation = possibleMeshes[meshToDisplay].transform.DORotate(Vector2.up * 180, rotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }


    void OnDestroy()
    {
        currentAnimation?.Kill();
    }

}

