using System;
using System.Collections;
using GGMLib.ObjectPool.Runtime;
using UnityEngine;

namespace Pool
{
    public abstract class AbstractEffectPool : AbstractMonoPoolable
    {
        [SerializeField] private PoolManagerSO poolManager;
        
        private ParticleSystem _particle;

        protected virtual void Awake()
        {
            _particle = GetComponentInChildren<ParticleSystem>();
        }

        public override void ResetItem()
        {
            base.ResetItem();
            _particle.Play();
        }

        public abstract void ExtraResetEvents();

        private IEnumerator ParticleDurationPush()
        {
            yield return new WaitForSeconds(_particle.main.duration);
            poolManager.Push(this);
        }
    }
}