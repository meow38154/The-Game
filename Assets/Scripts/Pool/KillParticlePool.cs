using Unity.Cinemachine;
using UnityEngine;

namespace Pool
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class KillParticlePool : AbstractEffectPool
    {
        private CinemachineImpulseSource _impulseSource;
        protected override void Awake()
        {
            base.Awake();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public override void ExtraResetEvents()
        {
            _impulseSource.GenerateImpulse();
        }
    }
}