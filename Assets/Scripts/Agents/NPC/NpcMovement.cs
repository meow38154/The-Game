using System;
using Agents.Players;
using DG.Tweening;
using GGMLib.AnimatorSystem;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace Agents.NPC
{
    public class NpcMovement : MonoBehaviour, IModule, IControlMovement
    {
        [SerializeField] private AnimParamSO animParamSo;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigid;

        private ModuleOwner _owner;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public bool CanManualMovement { get; set; }
        public void SetAutoVelocity(Vector3 velocity)
        {

        }

        private void Update()
        {
            SetMovementDirection(rigid.linearVelocity);
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            animator.SetBool(animParamSo.ParamHash, movementInput != Vector2.zero);
        }

        public void RotateTo(Vector3 direction)
        {

        }
    }
}