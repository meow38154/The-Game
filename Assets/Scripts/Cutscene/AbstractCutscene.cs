using System.Collections;
using Agents.Players;
using UnityEngine;
using Utility;

namespace Cutscene
{
    public abstract class AbstractCutscene : MonoBehaviour, IGetPlayer
    {
        public GameObject Player { get; set; }

        public void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }
        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
        }

        public void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        public void CutsceneStart()
        {
            StartCoroutine(CutsceneStartCoroutine());
        }
        
        protected abstract IEnumerator CutsceneStartCoroutine();
    }
}