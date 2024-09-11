using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



/// <summary>
/// Controls the "pipe" which is used to grab gold items
/// </summary>
public class PipeController : MonoBehaviour
{
    [SerializeField]
    float animationDuration = 1f;

    [SerializeField]
    GameObject player;

    LineRenderer lineRenderer;

    Sequence currentAnimation = null;

    public bool IsAnimating => currentAnimation != null;

    /// <summary>
    /// Starts the animation of grabbing the item. Invokes onComplete when the animation is finished
    /// </summary>
    public void GrabItem(GameObject item, Action onComplete)
    {
        var oldItemPos = item.transform.position;

        if (IsAnimating)
        {
            throw new InvalidOperationException("Cannot animate while already animating");
        }

        lineRenderer.enabled = true;

        currentAnimation = DOTween.Sequence();

        currentAnimation.Insert(
            0,
            DOTween.To(
                () => lineRenderer.GetPosition(1),
                (pos) => lineRenderer.SetPosition(1, pos),
                item.transform.position,
                animationDuration / 2
                ).From(player.transform.position)
                .SetEase(Ease.Linear)
                .OnUpdate(() => lineRenderer.SetPosition(0, player.transform.position))
        );

        float secondPartProgress = 0;

        currentAnimation.Insert(
            animationDuration / 2,
            DOTween.To(
            () => secondPartProgress,
            (t) => { secondPartProgress = t; },
            1,
            animationDuration / 2)
            .From(0)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                lineRenderer.SetPosition(0, player.transform.position);
                lineRenderer.SetPosition(1, Vector3.Lerp(oldItemPos, player.transform.position, secondPartProgress));
                item.transform.position = lineRenderer.GetPosition(1);
            })
        );

        currentAnimation.onComplete += () =>
        {
            currentAnimation = null;
            lineRenderer.enabled = false;
            onComplete.Invoke();
        };
    }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
}
