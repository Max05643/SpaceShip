using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using Zenject;
using UnityEngine.SceneManagement;


/// <summary>
/// Controls input from the player and ship's movement
/// </summary>
public class PlayerController : MonoBehaviour
{
    enum GameOverType
    {
        Victory,
        Health,
        Barrier
    }

    enum GameState
    {
        NotStarted,
        Playing,
        GameOver
    }

    readonly Dictionary<GameOverType, string> gameOverMessages = new Dictionary<GameOverType, string>
    {
        { GameOverType.Victory, "You Win!" },
        { GameOverType.Health, "You lose after the fatal collision!" },
        { GameOverType.Barrier, "You lose after crossing the simulation's broders!" }
    };



    [System.Serializable]
    public class Settings
    {
        public float maxSpeed = 5, acceleration = 1, rotationSpeed = 0.5f, speedAfterTheCollision = 10;
        public int initialHealth = 100, damageFromAsteroid = 10;

        public float distanceToWin = 2000;

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

    [Inject]
    PlanetController planetController;

    [Inject]
    ZenjectSceneLoader zenjectSceneLoader;

    Rigidbody rigidbodyComponent;

    Quaternion currentRotation;

    int currentHealth = 0;
    int coinsCount = 0;

    GameState gameState = GameState.NotStarted;

    GameOverType? gameOverType = null;

    [InjectOptional(Id = "GameLevelIndex")]
    int? levelIndex = null;

    void Awake()
    {
        currentHealth = settings.initialHealth;
        rigidbodyComponent = GetComponent<Rigidbody>();
        uiController.DisplayCoinsCount(null);
    }

    void FixedUpdate()
    {
        switch (gameState)
        {
            case GameState.NotStarted:
                ProcessStartInput();
                break;
            case GameState.Playing:
                ProcessPhysics();
                ProcessGrabInput();
                ApplyFovEffect();
                RepaintUI();
                CheckWinCondition();
                break;
            case GameState.GameOver:
                ProcessExitInput();
                break;
        }
    }

    void ProcessStartInput()
    {
        if (inputController.GetActionInput())
        {
            gameState = GameState.Playing;
            uiController.HideTutorial();
        }
    }

    void ProcessExitInput()
    {
        if (inputController.GetActionInput())
        {
            if (levelIndex == null)
            {
                levelIndex = 0;
            }

            int newLevel = gameOverType == GameOverType.Victory ? (1 - levelIndex.Value) : levelIndex.Value;

            zenjectSceneLoader.LoadScene(0, LoadSceneMode.Single, (container) =>
            {
                container.BindInstance(newLevel).WithId("GameLevelIndex").WhenInjectedInto<SettingsInstaller>();
                container.BindInstance(newLevel).WithId("GameLevelIndex").WhenInjectedInto<PlayerController>();
            });
        }
    }

    void CheckWinCondition()
    {
        if (transform.position.z >= settings.distanceToWin)
            EndGame(GameOverType.Victory);
    }

    void EndGame(GameOverType settedGameOverType)
    {
        gameOverType = settedGameOverType;
        gameState = GameState.GameOver;
        planetController.DisableMovement();
        uiController.ShowGameOver(gameOverMessages[settedGameOverType]);
        cameraController.FreezeCameraPosition();
        rigidbodyComponent.velocity = gameOverType == GameOverType.Victory ? Vector3.forward * settings.maxSpeed : Vector3.zero;
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
        else if (collision.gameObject.CompareTag("Barrier"))
            ProcessBarrierCollision();
    }

    void ProcessBarrierCollision()
    {
        EndGame(GameOverType.Barrier);
    }

    void ProcessAsteroidCollision()
    {
        currentHealth -= settings.damageFromAsteroid;
        rigidbodyComponent.velocity = -Vector3.forward * settings.speedAfterTheCollision;
        cameraController.ApplyDamageAnimation();
        soundController.PlayClip(0);

        if (currentHealth <= 0)
            EndGame(GameOverType.Health);
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
