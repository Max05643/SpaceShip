using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField]
    GameObject asteroidPrefab, goldPrefab;
    public override void InstallBindings()
    {
        Container.BindFactory<AsteroidController, AsteroidController.Factory>().FromComponentInNewPrefab(asteroidPrefab);
        Container.BindFactory<GoldController, GoldController.Factory>().FromComponentInNewPrefab(goldPrefab);

        Container.Bind<InputController>().AsSingle().NonLazy();
    }
}