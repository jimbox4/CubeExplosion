using System;
using Unity.VisualScripting;
using UnityEngine;

public class MouseClickRaycast : UserInput
{
    [SerializeField] private LayerMask _rayHitLayerMask;

    public override event Action<Cube> OnCubeClick;

    private Input _input;
    
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

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, _rayHitLayerMask))
        {
            Cube cube = hit.transform.GetComponent<Cube>();

            Debug.Log($"Click on {hit.transform.gameObject.name}\nGeneration = {cube.Generation}\n");

            OnCubeClick?.Invoke(cube);
        }
    }

}
