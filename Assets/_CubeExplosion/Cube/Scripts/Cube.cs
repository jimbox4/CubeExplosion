using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private const float ScaleCoefficient = 0.5f;
    private const float DevideChanceCoefficient = 0.5f;
    private const float MassCoefficient = 0.5f;
    private const float ParticleSizeCoefficient = 0.5f;
    private const float ParticleSpeedCoefficient = 0.5f;
    private const float ConstantForceCoefficient = 0.5f;
    private const float ExplosionForceCoefficient = 0.7f;

    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private ConstantForce _constantForce;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRaduis = 10000;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private AudioSource _explosionAudioSource;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private MeshRenderer _meshRederer;

    private float _destroyDelay = 3;
    private float _maxChance = 100;
    private float _devideChance = 100;

    private int _maxCountCubes = 6;
    private int _minCountCubes = 2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _meshRederer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float devideChance, Vector3 scale, Vector3 createrTransform, float mass, float particleSize, float particleSpeed, float constForceY, float explosionForce)
    {
        _devideChance = devideChance;
        transform.localScale = scale;
        _rigidbody.mass = mass;
        _explosionEffect.startSize = particleSize;
        _explosionEffect.startSpeed = particleSpeed;
        _constantForce.force = new Vector3(0, constForceY, 0); 
        _explosionForce = explosionForce;

        _rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
        _meshRederer.enabled = true;

        _meshRederer.material.color = GetRandomColor();

        _rigidbody.AddExplosionForce(_explosionForce, createrTransform, _explosionRaduis);
    }

    public void Devide()
    {
        if (Random.Range(0, _maxChance + 1) <= _devideChance)
        {
            Split();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Split()
    {
        int countCubes = Random.Range(_minCountCubes, _maxCountCubes + 1);

        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
        _explosionEffect.Play();
        _meshRederer.enabled = false;

        _explosionAudioSource.Play();
        Debug.Log($"Count of new cubes = {countCubes}");

        for (int i = 0; i < countCubes; i++)
        {
            Cube cube = Instantiate(_cubePrefab, GetCubeRandomPosition(transform.position, -0.3f, 0.3f), Quaternion.identity);
            cube.Initialize(_devideChance * DevideChanceCoefficient,
                transform.localScale * ScaleCoefficient,
                transform.position,
                _rigidbody.mass * MassCoefficient, 
                _explosionEffect.startSize * ParticleSizeCoefficient,
                _explosionEffect.startSpeed * ParticleSpeedCoefficient,
                _constantForce.force.y * ConstantForceCoefficient,
                _explosionForce * ExplosionForceCoefficient);
        }

        Destroy(gameObject, _destroyDelay);
    }

    private Vector3 GetCubeRandomPosition(Vector3 centerPosition, float minValue, float maxValue)
    {
        float x = Random.Range(centerPosition.x - minValue, centerPosition.x + maxValue);
        float y = Random.Range(centerPosition.y - minValue, centerPosition.y + maxValue);
        float z = Random.Range(centerPosition.z - minValue, centerPosition.z + maxValue);

        return new Vector3(x, y, z);
    }

    private Color GetRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue, 1);
    }
}
