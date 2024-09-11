using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using Zenject;
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        public float maxSpeed = 5, acceleration = 1, rotationSpeed = 0.5f, speedAfterTheCollision = 10;
    }

    [Inject]
    Settings settings;

    [SerializeField]
    Transform playerModelObject;

    [SerializeField]
    [Range(10, 90)]
    float maxRotationAngle = 80;

    [Inject]
    GoldDetector goldDetector;

    [Inject]
    CameraController cameraController;

    [Inject]
    InputController inputController;

    Rigidbody rigidbodyComponent;

    Quaternion currentRotation;

    void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ProcessPhysics();
        ProcessGrabInput();
        ApplyFovEffect();
    }

    void OnCollisionEnter(Collision collision)
    {
        rigidbodyComponent.velocity = -Vector3.forward * settings.speedAfterTheCollision;
        cameraController.ApplyDamageAnimation();
    }

    void ProcessGrabInput()
    {
        if (inputController.GetActionInput())
        {
            goldDetector.GrabNearestGold();
        }
    }

    void ProcessPhysics()
    {
        var deltaTime = Time.fixedDeltaTime;

        var directionInput = inputController.GetDirectionInput();

        var targetRotation = Quaternion.Euler(directionInput.y * maxRotationAngle, directionInput.x * maxRotationAngle, 0);
        currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, settings.rotationSpeed * deltaTime);


        playerModelObject.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, 0, -currentRotation.eulerAngles.y);

        var targetDirection = currentRotation * Vector3.forward;


        rigidbodyComponent.velocity = Vector3.MoveTowards(rigidbodyComponent.velocity, targetDirection * settings.maxSpeed, deltaTime * settings.acceleration);
    }

    void ApplyFovEffect()
    {
        var velocity = rigidbodyComponent.velocity.z;
        var t = Mathf.Clamp01(Mathf.InverseLerp(0, settings.maxSpeed, velocity));
        cameraController.ApplyFovEffect(t);
    }

}
