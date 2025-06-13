using Features.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Exploder
{
    private const int MinDistance = 1;

    private Vector3 _explosionCenter;
    private float _force;
    private float _radius;

    public void Explode(List<Rigidbody> rigidbodies, Vector3 explosionCenter, float force, float radius, ExplosionMode mode)
    {
        _explosionCenter = explosionCenter;
        _force = force;
        _radius = radius;

        foreach (var rigidbody in rigidbodies)
        {
            SwitchMode(rigidbody, mode);
        }
    }

    public void Explode(Rigidbody rigidbody, Vector3 explosionCenter, float force, float radius, ExplosionMode mode)
    {
        _explosionCenter = explosionCenter;
        _force = force;
        _radius = radius;

        SwitchMode(rigidbody, mode);
    }

    private void SwitchMode(Rigidbody rigidbody, ExplosionMode mode)
    {
        switch (mode)
        {
            case ExplosionMode.None:
                Explode(rigidbody);
                break;
            case ExplosionMode.Linear:
                ExplodeLinear(rigidbody);
                break;
            default:
                break;
        }
    }

    private void Explode(Rigidbody rigidbody)
    {
        rigidbody.AddExplosionForce(_force, _explosionCenter, _radius);
    }

    private void ExplodeLinear(Rigidbody rigidbody)
    {
        float distance = Vector3.Distance(rigidbody.transform.position, _explosionCenter);

        if (distance > _radius)
        {
            return;
        }

        distance = Mathf.Clamp(distance, MinDistance, _radius);

        float distanceForce = Utils.Math.LinearDecayForDistance(_force, distance, _radius);

        rigidbody.AddExplosionForce(distanceForce, _explosionCenter, _radius);
    }
}

public enum ExplosionMode
{
    None,
    Linear
}
