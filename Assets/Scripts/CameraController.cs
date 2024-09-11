using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Transform cameraSystem;

    [SerializeField]
    [Range(10, 90)]
    float minFov, maxFov;
    public void ApplyFovEffect(float intensity)
    {
        mainCamera.fieldOfView = Mathf.Lerp(minFov, maxFov, intensity);
    }
    Sequence currentAnimation = null;

    public void ApplyDamageAnimation()
    {
        PostProcessVolume m_Volume;
        Vignette m_Vignette;
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(1f);
        m_Vignette.smoothness.Override(1f);
        m_Vignette.roundness.Override(1f);
        m_Vignette.color.Override(Color.red);

        m_Volume = PostProcessManager.instance.QuickVolume(6, 100f, m_Vignette);
        m_Volume.weight = 0f;

        currentAnimation?.Kill(true);

        currentAnimation = DOTween.Sequence()
           .Append(DOTween.To(() => m_Volume.weight, x => m_Volume.weight = x, 1f, 0.25f).From(0))
           .AppendInterval(1f)
           .Append(DOTween.To(() => m_Volume.weight, x => m_Volume.weight = x, 0f, 0.25f).From(1))
           .OnComplete(() =>
           {
               RuntimeUtilities.DestroyVolume(m_Volume, true, true);
           });

        cameraSystem.DOKill(true);
        cameraSystem.DOShakeRotation(0.5f, 10, 90, 90, true);
    }

}
