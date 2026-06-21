using System;
using Agents.Players;
using DG.Tweening;
using GGMLib.ModuleSystem;
using Sound;
using UI.Dialogues;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.MainMenu
{
    public class StartAnimation : MonoBehaviour, IGetPlayer
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private SoundClipSO sound;
        
        public GameObject Player { get; set; }

        public void AnimationPlay()
        {
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
            Camera cam = Camera.main;
            if (cam == null) return; 
            CinemachineBrain brain = cam.GetComponent<CinemachineBrain>();
            if (brain == null) return;
            CinemachineCamera cine = brain.ActiveVirtualCamera as  CinemachineCamera;
            if (cine == null) return;
            
            Sequence s = DOTween.Sequence();
            
            s.Append(cine.transform.DORotate(new Vector3(30f, 0f, 0f), 0.5f).SetEase(Ease.OutSine));
            s.AppendCallback(() =>
            {
                imageBackground.DOFade(0, 0.5f);
                dialoguePanel.SetActive(true);
                Player.GetComponent<ModuleOwner>().GetModule<IControlMovement>().CanManualMovement = true;
            });
            s.AppendInterval(0.5f);
            s.AppendCallback(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Exit()
        {
            Application.Quit();
        }
        
        private void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
        }
    }
}