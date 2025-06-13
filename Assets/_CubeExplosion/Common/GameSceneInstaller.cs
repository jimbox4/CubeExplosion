using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private UserInput _userInput;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private SpawnPoint _spawnPoint;

    public override void InstallBindings()
    {
        BindInput();

        Container.Bind<Exploder>().AsSingle().NonLazy();
        Container.Bind<Cube>().FromInstance(_cubePrefab).NonLazy();
        Container.Bind<ISpawnPoint>().FromInstance(_spawnPoint).NonLazy();

        Container.BindInterfacesTo<CubeHandler>().AsSingle();
    }

    private void BindInput()
    {
        Container
            .Bind<UserInput>()
            .FromComponentsInNewPrefab(_userInput)
            .AsSingle()
            .NonLazy();
    }
}