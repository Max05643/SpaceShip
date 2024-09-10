using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 5, acceleration = 1, rotationSpeed = 0.5f, speedAfterTheCollision = 10;

    [SerializeField]
    Transform playerModelObject;

    [SerializeField]
    [Range(10, 90)]
    float maxRotationAngle = 80;

    [SerializeField]
    [Range(10, 90)]
    float minFov, maxFov;

    [SerializeField]
    Camera mainCamera;

    Rigidbody rigidbodyComponent;

    Quaternion currentRotation;

    void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ProcessInput();
        ApplyFovEffect();
    }


    Sequence currentAnimation = null;
    void OnCollisionEnter(Collision collision)
    {
        rigidbodyComponent.velocity = -Vector3.forward * speedAfterTheCollision;

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

    }

    void ProcessInput()
    {
        var deltaTime = Time.fixedDeltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = -Input.GetAxis("Vertical");

        var targetRotation = Quaternion.Euler(vertical * maxRotationAngle, horizontal * maxRotationAngle, 0);
        currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * deltaTime);



        playerModelObject.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, 0, -currentRotation.eulerAngles.y);

        var targetDirection = currentRotation * Vector3.forward;



        rigidbodyComponent.velocity = Vector3.MoveTowards(rigidbodyComponent.velocity, targetDirection * maxSpeed, deltaTime * acceleration);
    }

    void ApplyFovEffect()
    {
        var velocity = rigidbodyComponent.velocity.magnitude;
        var t = Mathf.InverseLerp(0, maxSpeed, velocity);
        mainCamera.fieldOfView = Mathf.Lerp(minFov, maxFov, t);
    }

}
