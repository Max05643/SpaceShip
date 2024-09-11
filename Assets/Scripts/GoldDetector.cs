using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


/// <summary>
/// Controller for detecting gold items and grabbing them
/// </summary>
public class GoldDetector : MonoBehaviour
{

    [Serializable]
    public class Settings
    {
        public float detectionRadius = 1f;
        public float tolerationDistance = 10;
    }

    [Inject]
    Settings settings;

    [Inject]
    PipeController pipeController;

    [Inject]
    GoldSpawnController goldSpawnController;

    [SerializeField]
    Transform player;

    GameObject targetGold = null;


    /// <summary>
    /// Starts the animation of grabbing the nearest gold if it is possible
    /// </summary>
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


    /// <summary>
    /// Returns the possible gold items that can be grabbed
    /// </summary>
    IEnumerable<GameObject> GetPossibleGolds()
    {
        return goldSpawnController.CurrentGolds.
        Where(
            g => Vector3.Distance(player.position, g.transform.position) < settings.detectionRadius
            && g.transform.position.z > player.position.z + settings.tolerationDistance
            && !g.GetComponent<GoldController>().IsBeingGrabbed
            );
    }

    void Update()
    {
        UpdateDetected();
    }

    /// <summary>
    /// Updates the detected gold items' visual effects
    /// </summary>
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
