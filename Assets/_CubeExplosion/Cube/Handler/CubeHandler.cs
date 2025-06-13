using Features.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CubeHandler : IInitializable, IDisposable
{
    private const float BaseScale = 7.0f;
    private const float BaseDevideChance = 100f;
    private const float BaseMass = 4f;
    private const float BaseParticleSize = 10f;
    private const float BaseParticleSpeed = 100f;
    private const float BaseConstantForce = -60f;
    private const float BaseExplosionForce = 10000f;

    private const float ScaleCoefficient = 0.5f;
    private const float DevideChanceCoefficient = 0.5f;
    private const float MassCoefficient = 0.5f;
    private const float ParticleSizeCoefficient = 0.5f;
    private const float ParticleSpeedCoefficient = 0.5f;
    private const float ConstantForceCoefficient = 0.5f;
    private const float ExplosionForceCoefficient = 0.7f;

    private ISpawnPoint _spawnPoint;

    private Spawner<Cube> _spawner = new Spawner<Cube>();
    private List<Cube> _cubes = new List<Cube>();
    private UserInput _userInput;
    private Exploder _exploder;
    private Cube _cubePrefab;

    private float _cubeExplosionRadius = 18f;
    private float _cubeDestroyDelay = 3;
    private int _maxCountCubes = 6;
    private int _minCountCubes = 2;

    [Inject]
    public void Construct(UserInput mouseClickRaycast, Exploder exploder, Cube cubePrefab, ISpawnPoint spawnPoint)
    {
        _userInput = mouseClickRaycast;
        _exploder = exploder;
        _cubePrefab = cubePrefab;
        _spawnPoint = spawnPoint;
    }

    public void Initialize()
    {
        _userInput.OnCubeClick += MakeCubeAction;

        Cube instance = _spawner.Spawn(_cubePrefab, _spawnPoint.GetPosition());
        InitializeCube(instance, instance.Generation);
    }

    private void MakeCubeAction(Cube cube)
    {
        float currentDevideChance = Utils.Math.ExponentialDecay(BaseDevideChance,DevideChanceCoefficient, cube.Generation);

        if (Utils.Randomizer.TryChance(currentDevideChance))
        {
            DevideCube(cube);
        }
        else
        {
            ExplodeCube(cube);
        }
    }

    private void DevideCube(Cube cube)
    {
        _cubes.Remove(cube);

        cube.BecomeFreezeInvisible();
        cube.PlayExposionEffect();

        int spawnCount = UnityEngine.Random.Range(_minCountCubes, _maxCountCubes);

        int nextCubeGeneration = cube.Generation + 1;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPoint = Utils.Randomizer.GetRandomPosition(cube.transform.position, -0.3f, 0.3f);
            Cube instance = _spawner.Spawn(_cubePrefab, spawnPoint);


            InitializeCube(instance, nextCubeGeneration);
            _cubes.Add(instance);
        }

        List<Rigidbody> rigidbodies = _cubes.TakeLast(spawnCount - 1).Select(currentCube => currentCube.Rigidbody).ToList();

        float explosionForce = Utils.Math.ExponentialDecay(BaseExplosionForce, ExplosionForceCoefficient, cube.Generation);
        _exploder.Explode(rigidbodies, cube.transform.position, explosionForce, _cubeExplosionRadius, ExplosionMode.None);

        cube.Destroy(_cubeDestroyDelay);
    }

    private void ExplodeCube(Cube cube)
    {
        _cubes.Remove(cube);

        cube.BecomeFreezeInvisible();
        cube.PlayExposionEffect();

        List<Rigidbody> rigidbodies = _cubes.Select(currentCube => currentCube.Rigidbody).ToList();

        float explosionForce = Utils.Math.ExponentialDecay(BaseExplosionForce, ExplosionForceCoefficient, cube.Generation);
        _exploder.Explode(rigidbodies, cube.transform.position, explosionForce, _cubeExplosionRadius, ExplosionMode.Linear);

        cube.Destroy(_cubeDestroyDelay);
    }

    private void InitializeCube(Cube cube, int generation)
    {
        cube.SetRandomColor();

        float currentScale = Utils.Math.ExponentialDecay(BaseScale, ScaleCoefficient, generation);
        float currentMass = Utils.Math.ExponentialDecay(BaseMass, MassCoefficient, generation);
        float currentParticleSize = Utils.Math.ExponentialDecay(BaseParticleSize, ParticleSizeCoefficient, generation);
        float currentParticleSpeed = Utils.Math.ExponentialDecay(BaseParticleSpeed, ParticleSpeedCoefficient, generation);
        float currentForceY = Utils.Math.ExponentialDecay(BaseConstantForce, ConstantForceCoefficient, generation);

        cube.Initialize(
            generation,
            currentScale,
            currentMass,
            currentParticleSize,
            currentParticleSpeed,
            currentForceY);
    }

    public void Dispose()
    {
        _userInput.OnCubeClick -= MakeCubeAction;
    }
}
