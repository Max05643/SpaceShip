using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldDetector : MonoBehaviour
{
    [SerializeField]
    float detectionRadius = 1f;

    [SerializeField]
    PipeController pipeController;

    [SerializeField]
    Transform player;


    List<GameObject> golds = new List<GameObject>();

    void Start()
    {
        GetComponent<SphereCollider>().radius = detectionRadius;
    }

    public void GrabNearestGold()
    {
        if (golds.Count == 0 || pipeController.IsAnimating)
        {
            return;
        }

        var playerPos = player.position;

        GameObject nearestGold = golds.Where(g => g.transform.position.z > player.position.z).OrderBy(g => Vector3.Distance(playerPos, g.transform.position)).First();


        golds.Remove(nearestGold);
        UpdateDetected();


        nearestGold.GetComponent<GoldController>().StartGrabbing();

        pipeController.GrabItem(nearestGold, () =>
        {
            nearestGold.GetComponent<GoldController>().FinishGrabbing();
        });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            golds.Add(other.gameObject);
            UpdateDetected();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            golds.Remove(other.gameObject);
            UpdateDetected();
        }
    }

    void UpdateDetected()
    {
        var detectedOne = golds.Where(g => g.transform.position.z > player.position.z).OrderBy(g => Vector3.Distance(transform.position, g.transform.position)).FirstOrDefault();
        var canAnytingBeDetected = !pipeController.IsAnimating;

        foreach (var gold in golds)
        {
            if (gold == detectedOne && canAnytingBeDetected)
            {
                gold.GetComponent<GoldController>().StartDetecting();
            }
            else
            {
                gold.GetComponent<GoldController>().StopDetecting();
            }
        }
    }

}
