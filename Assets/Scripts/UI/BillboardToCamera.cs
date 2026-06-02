using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    private Camera _targetCamera;

    private Transform _cameraTransform;

    private void Awake()
    {
        if (_targetCamera == null)
            _targetCamera = Camera.main;

        if (_targetCamera != null)
            _cameraTransform = _targetCamera.transform;
    }

    private void LateUpdate()
    {
        if (_cameraTransform == null)
            return;

        Vector3 direction = transform.position - _cameraTransform.position;

        if (direction.sqrMagnitude <= 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}