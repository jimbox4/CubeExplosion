using System;
using UnityEngine;

public class MouseClickRaycast : UserInput
{
    [SerializeField] private LayerMask _rayHitLayerMask;

    public override event Action<Cube> CubeClicked;

    private Input _input;
    private float _rayDistance = 1000f;

    public void Awake()
    {
        _input = new Input();

        _input.User.Click.performed += click => CastRay();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _rayHitLayerMask))
        {
            Cube cube = hit.transform.GetComponent<Cube>();

            Debug.Log($"Click on {hit.transform.gameObject.name}\nGeneration = {cube.Generation}\n");

            CubeClicked?.Invoke(cube);
        }
    }

}
