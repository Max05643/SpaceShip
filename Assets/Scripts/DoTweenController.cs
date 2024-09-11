using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class DoTweenController
{
    public DoTweenController()
    {
        DOTween.SetTweensCapacity(10000, 10000);
    }
}
