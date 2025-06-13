using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class Cube : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private AudioSource _explosionAudioSource;
    
    public Rigidbody Rigidbody { get; private set; }
    public int Generation { get; private set; } = 0;

    private Collider _collider;
    private MeshRenderer _meshRederer;
    private ConstantForce _constantForce;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _meshRederer = GetComponent<MeshRenderer>();
        _constantForce = GetComponent<ConstantForce>();
    }

    public void Initialize(int generation, float scale, 
        float mass, float particleSize,
        float particleSpeed, float constForceY)
    {
        Generation = generation;
        transform.localScale = Vector3.one * scale;
        Rigidbody.mass = mass;
        _explosionEffect.startSize = particleSize;
        _explosionEffect.startSpeed = particleSpeed;
        _constantForce.force = new Vector3(0, constForceY, 0);

        Rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
        _meshRederer.enabled = true;
    }

    public void BecomeFreezeInvisible()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
        _meshRederer.enabled = false;
    }

    public void BecomeUnfreezeVisible()
    {
        Rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
        _meshRederer.enabled = true;
    }

    public void PlayExposionEffect()
    {
        _explosionEffect.Play();
        _explosionAudioSource.Play();
    }

    public void Destroy(float destroyDelay)
    {
        Destroy(gameObject, destroyDelay);
    }

    public Vector3 GetCubeRandomPosition(Vector3 centerPosition, float minValue, float maxValue)
    {
        float x = Random.Range(centerPosition.x - minValue, centerPosition.x + maxValue);
        float y = Random.Range(centerPosition.y - minValue, centerPosition.y + maxValue);
        float z = Random.Range(centerPosition.z - minValue, centerPosition.z + maxValue);

        return new Vector3(x, y, z);
    }

    public void SetRandomColor()
    {
        _meshRederer.material.color = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue, 1);
    }
}