using System;
using UnityEngine;
using UnityEngine.Events;

namespace Boss
{
    public class RemoveCountDown : MonoBehaviour
    {
        [SerializeField] private float time;
        [SerializeField] private UnityEvent onEnd;
        
        public bool Start { get; set; }

        private bool _end;
        
        private void Update()
        {
            if (!Start) return;
            
            time -= Time.deltaTime;
            if (_end || time > 0) return;
            _end = true;
            onEnd?.Invoke();
        }
    }
}