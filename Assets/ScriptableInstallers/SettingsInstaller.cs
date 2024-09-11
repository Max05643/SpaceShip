using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [System.Serializable]
    public class Settings
    {
        public PlayerController.Settings playerSettings;

        public PlanetController.Settings planetSettings;

        public BordersController.Settings bordersSettings;

        public AsteroidsController.Settings asteroidsSettings;

        public GoldSpawnController.Settings goldSpawnSettings;
    }

    [SerializeField]
    Settings[] scenesSettings;

    [InjectOptional(Id = "GameLevelIndex")]
    int? levelIndex = null;


    public override void InstallBindings()
    {
        if (levelIndex == null)
        {
            levelIndex = 0;
        }

        var sceneSettings = scenesSettings[levelIndex.Value];


        Container.BindInstance(sceneSettings.playerSettings);
        Container.BindInstance(sceneSettings.planetSettings);
        Container.BindInstance(sceneSettings.bordersSettings);
        Container.BindInstance(sceneSettings.asteroidsSettings);
        Container.BindInstance(sceneSettings.goldSpawnSettings);
    }
}