using System;
using UnityEngine;

public abstract class UserInput : MonoBehaviour
{
    public abstract event Action<Cube> OnCubeClick;
}
