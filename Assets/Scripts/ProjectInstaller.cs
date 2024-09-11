using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    GameObject soundManagerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<SoundController>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle().NonLazy();
        Container.Bind<DoTweenController>().AsSingle().NonLazy();
    }
}