using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> where T : MonoBehaviour
{
    public T Spawn(T prefab, Vector3 position)
    {
        T instance = GameObject.Instantiate(prefab, position, Quaternion.identity);

        return instance;
    }

    public List<T> Spawn(T prefab, Vector3 position, int count)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < count; i++)
        {
            T instance = GameObject.Instantiate(prefab, position, Quaternion.identity);
            list.Add(instance);
        }

        return list;
    }
}
