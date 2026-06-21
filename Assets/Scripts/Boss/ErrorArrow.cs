using System;
using Agents.Players;
using Unity.Cinemachine;
using UnityEngine;
using Utility;

namespace Boss
{
    public class ErrorArrow : MonoBehaviour, IGetPlayer
    {
        [SerializeField] private ParticleSystem errorArrowStart;
        [SerializeField] private float speed;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        public GameObject Player { get; set; }
        private void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void OnEnable()
        {
            errorArrowStart.Play();
            impulseSource.GenerateImpulse();
        }

        private float _time;
        
        private void Update()
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            
            _time += Time.deltaTime;
            if (_time > 5)
            {
                Destroy(gameObject);
            }
        }

        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }
    }
}