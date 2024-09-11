using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GoldDetector : MonoBehaviour
{
    [SerializeField]
    float detectionRadius = 1f;

    [SerializeField]
    float tolerationDistance = 10;

    [Inject]
    PipeController pipeController;

    [Inject]
    GoldSpawnController goldSpawnController;

    [SerializeField]
    Transform player;

    GameObject targetGold = null;

    public void GrabNearestGold(Action onSuccessfulGrab)
    {
        if (targetGold == null || pipeController.IsAnimating)
        {
            return;
        }

        GameObject nearestGold = targetGold;

        nearestGold.GetComponent<GoldController>().StartGrabbing();

        pipeController.GrabItem(nearestGold, () =>
        {
            nearestGold.GetComponent<GoldController>().FinishGrabbing();
            onSuccessfulGrab.Invoke();
        });
    }

    IEnumerable<GameObject> GetPossibleGolds()
    {
        return goldSpawnController.CurrentGolds.
        Where(
            g => Vector3.Distance(player.position, g.transform.position) < detectionRadius
            && g.transform.position.z > player.position.z + tolerationDistance
            && !g.GetComponent<GoldController>().IsBeingGrabbed
            );
    }

    void Update()
    {
        UpdateDetected();
    }

    void UpdateDetected()
    {
        targetGold?.GetComponent<GoldController>().StopDetecting();
        targetGold = null;

        var gold = GetPossibleGolds().OrderBy(g => Vector3.Distance(g.transform.position, player.transform.position)).FirstOrDefault();

        if (gold != null)
        {
            targetGold = gold;
            targetGold.GetComponent<GoldController>().StartDetecting();
        }
    }

}
