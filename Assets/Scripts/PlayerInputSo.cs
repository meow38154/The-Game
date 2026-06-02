using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "player Input", menuName = "SO/Player input", order = 0)]
public class PlayerInputSo : ScriptableObject, Controls.IPlayerActions
{
    [SerializeField] private LayerMask whatIsGround;
    public Vector2 MoveDir { get; private set; }
    
    private Camera _mainCam;
    private Vector3 _worldMousePosition;
    private Vector2 _screenMousePosition;
    private Controls _controls;

    public event Action OnMouseClickEvent;
    
    public Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }
    }
        
    public Vector3 GetWorldMousePosition()
    {
        if (MainCam == null)
            return _worldMousePosition;
            
        Ray camRay = MainCam.ScreenPointToRay(_screenMousePosition);
        if (Physics.Raycast(camRay, out RaycastHit hit , MainCam.farClipPlane, whatIsGround))
        {
            _worldMousePosition = hit.point;
        }
        return _worldMousePosition;
    }
    
    
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        if(_controls != null)
            _controls.Player.Disable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }

    public void OnPointer(InputAction.CallbackContext context)
    {
        _screenMousePosition = context.ReadValue<Vector2>();
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMouseClickEvent?.Invoke();
        }        
    }
}