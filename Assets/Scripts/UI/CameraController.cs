using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100.0f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform rotationTarget; // Set this to the object Cinemachine follows

    private InputAction _lookAction;
    private InputAction _mouseWheelPressAction;
    private bool _isRotating = false;

    private void Awake()
    {
        _lookAction = playerInput.actions["Look"];
        _mouseWheelPressAction = playerInput.actions["MouseWheelPress"];

        _lookAction.Enable();
        _mouseWheelPressAction.Enable();

        _mouseWheelPressAction.performed += _ => _isRotating = true;
        _mouseWheelPressAction.canceled += _ => _isRotating = false;
    }

    private void Update()
    {
        if (_isRotating && rotationTarget != null)
        {
            Vector2 mouseDelta = _lookAction.ReadValue<Vector2>();

            float rotationX = mouseDelta.x * rotationSpeed * Time.deltaTime;
            rotationTarget.Rotate(0, rotationX, 0, Space.World);
        }
    }

    private void OnDestroy()
    {
        _lookAction.Dispose();
        _mouseWheelPressAction.Dispose();
    }
}
