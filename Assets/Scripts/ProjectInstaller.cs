using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    GameObject soundManagerPrefab, musicManagerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<MusicController>().FromComponentInNewPrefab(musicManagerPrefab).AsSingle().NonLazy();
        Container.Bind<SoundController>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle().NonLazy();
        Container.Bind<DoTweenController>().AsSingle().NonLazy();
    }
}