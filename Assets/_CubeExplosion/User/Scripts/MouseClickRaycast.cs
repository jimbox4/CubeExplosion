using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class MouseClickRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask _rayHitLayerMask;

    private UserInput _input;
    
    private void Awake()
    {
        _input = new UserInput();

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, _rayHitLayerMask))
        {
            Debug.Log($"Click on {hit.transform.gameObject.name}");
            Cube cube = hit.transform.GetComponent<Cube>();

            cube.Devide();
        }
    }
}
