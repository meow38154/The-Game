using DG.Tweening;
using GGMLib.ModuleSystem;
using UnityEngine;
using Utility;

namespace Agents.Players
{
    public class PlayerMovement : MonoBehaviour, IModule, IControlMovement
    {
        [SerializeField] private float rotationMultiplier = -1;
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private float rotateDuration = 0.15f;

        private Vector3 _velocity;
        private Vector3 _movementDirection;
        private Vector3 _autoVelocity;
        private ModuleOwner _owner;
        
        private Tween _rotateTween;

        public Vector3 Velocity => _velocity;

        public bool CanManualMovement { get; set; } = true;


        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }
        

        public void AfterInit()
        {
        }


        public void SetAutoVelocity(Vector3 velocity)
        {
            _autoVelocity = velocity;
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection =
                new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        }

        private int _rotationY;
        public void RotateTo(Vector3 direction)
        {

            if (direction.x > 0.1f)
            {
                _rotationY = 180;
            }            
            else if (direction.x < -0.1f)
            {
                _rotationY = 0;
            }

            _rotateTween?.Kill();

            _rotateTween = _owner.transform
                .DORotate(
                    new Vector3(0f, _rotationY * rotationMultiplier, 0f),
                    rotateDuration)
                .SetEase(Ease.OutQuad);
        }

        private void FixedUpdate()
        {
            CalculateMovement();

            RotateTo(_velocity);

            MoveCharacter();
        }

        private void CalculateMovement()
        {
            Vector3 manualVelocity =
                CanManualMovement
                    ? _movementDirection * moveSpeed
                    : Vector3.zero;

            _velocity = manualVelocity + _autoVelocity;
        }

        private void MoveCharacter()
        {
            rigid.linearVelocity = _velocity;
        }

        private void OnDestroy()
        {
            _rotateTween?.Kill();
        }
    }
}