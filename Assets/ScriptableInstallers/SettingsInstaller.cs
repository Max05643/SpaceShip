using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [SerializeField]
    PlayerController.Settings playerSettings;

    [SerializeField]
    PlanetController.Settings planetSettings;

    [SerializeField]
    BordersController.Settings bordersSettings;

    [SerializeField]
    AsteroidsController.Settings asteroidsSettings;

    [SerializeField]
    GoldSpawnController.Settings goldSpawnSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(playerSettings);
        Container.BindInstance(planetSettings);
        Container.BindInstance(bordersSettings);
        Container.BindInstance(asteroidsSettings);
        Container.BindInstance(goldSpawnSettings);
    }
}