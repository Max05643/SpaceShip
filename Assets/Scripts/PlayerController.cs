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
        public int initialHealth = 100, damageFromAsteroid = 10;

        public bool displayCollectedCoins = true;
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

    [Inject]
    UIController uiController;

    [Inject]
    SoundController soundController;

    Rigidbody rigidbodyComponent;

    Quaternion currentRotation;

    int currentHealth = 0;
    int coinsCount = 0;

    void Awake()
    {
        currentHealth = settings.initialHealth;
        rigidbodyComponent = GetComponent<Rigidbody>();
        uiController.DisplayCoinsCount(null);
    }

    void FixedUpdate()
    {
        ProcessPhysics();
        ProcessGrabInput();
        ApplyFovEffect();
        RepaintUI();

    }

    void RepaintUI()
    {
        uiController.UpdateHealth((float)currentHealth / settings.initialHealth);
        var t = Mathf.Clamp01(Mathf.InverseLerp(0, settings.maxSpeed, rigidbodyComponent.velocity.z));
        uiController.UpdateSpeed(t);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
            ProcessAsteroidCollision();
    }

    void ProcessAsteroidCollision()
    {
        currentHealth -= settings.damageFromAsteroid;
        rigidbodyComponent.velocity = -Vector3.forward * settings.speedAfterTheCollision;
        cameraController.ApplyDamageAnimation();
        soundController.PlayClip(0);
    }

    void ProcessGrabInput()
    {
        if (inputController.GetActionInput())
        {
            goldDetector.GrabNearestGold(OnGrabbedCoint);
        }
    }


    void OnGrabbedCoint()
    {
        soundController.PlayClip(1);
        coinsCount++;
        if (settings.displayCollectedCoins)
            uiController.DisplayCoinsCount(coinsCount);
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
