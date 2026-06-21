using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Map
{
    public class WallDestroy : MonoBehaviour
    {
        [SerializeField] private Transform trm;
        private bool _reach;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Player")
            {
                _reach = true;
            }
        }

        private int _c;
        private bool _a;
        
        private void Update()
        {
            if (!_reach || !Keyboard.current.spaceKey.wasPressedThisFrame) return;
            _c++;
            if (_c < 10 || _a) return;
            
            Bug();
            _a = true;
            _c = 0;
        }

        public void Bug()
        {
            Sequence seq = DOTween.Sequence();
            
            seq.Append(trm.DOMove(trm.position + trm.up, 0f));
            seq.AppendInterval(1f);
            
            seq.Append(trm.DOMove(trm.position - trm.forward, 0f));
            seq.AppendInterval(0.05f);
            seq.Append(trm.DOMove(trm.position + trm.forward, 0f));
            seq.AppendInterval(0.05f);            
            seq.Append(trm.DOMove(trm.position - trm.forward, 0f));
            seq.AppendInterval(0.05f);
            seq.Append(trm.DOMove(trm.position + trm.forward, 0f));
            seq.AppendInterval(0.05f);            
            seq.Append(trm.DOMove(trm.position - trm.forward, 0f));
            seq.AppendInterval(0.05f);
            seq.Append(trm.DOMove(trm.position + trm.forward, 0f));
            seq.AppendInterval(0.05f);
            
            seq.AppendInterval(1f);
            seq.AppendCallback(()
                =>
            {
                trm.position = new Vector3(trm.position.x, 0, trm.position.z);
                GetComponent<Collider>().enabled = false;
                _a = false;
            });
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.name == "Player")
            {
                _reach = false;
            }
        }
    }
}