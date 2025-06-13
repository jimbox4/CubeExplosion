using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
