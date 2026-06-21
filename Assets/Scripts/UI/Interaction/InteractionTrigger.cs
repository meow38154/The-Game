using System;
using DG.Tweening;
using GGMLib.ModuleSystem;
using SaveSystems;
using TMPro;
using UI.Interaction.Events;
using UI.Interaction.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Interaction
{
    public class InteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        [Header("Start Setting")] 
        [SerializeField] private bool dead;
        [SerializeField] private Transform owner;
        
        [Header("Text Settings")]
        [SerializeField] private string nameText;        
        [SerializeField] private string loreText;

        [Header("Channel Settings")] 
        [SerializeField] private bool saveChannelNumber;
        [SerializeField] private int channelNumber;
        
        [Header("Interaction Settings")]
        [SerializeField] private int remainingIterations = 1;
        [SerializeField] private float interactionDuration = 2f;

        
        [Header("Visual Settings")]
        [SerializeField] private float animationDuration = 0.5f;

        public int ChannelNumber => channelNumber;
        public Transform Owner => owner;
        
        private Transform _panel;
        private Image _interactionLoad;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _loreText;
        
        private bool _enable;
        private bool _isInteracting;

        public event Action<int> OnInteractionTrigger; 
        
        
        private void Awake()
        {
            _panel = transform.Find("Panel");
            _interactionLoad = _panel.Find("EButtonPanel/InteractionLoad").TryGetComponent(out Image image) ?  image : null;
            Debug.Assert(image != null,  nameof(image) + " != null");
            _nameText = _panel.Find("Name").TryGetComponent(out TextMeshProUGUI nameTextMesh) ?  nameTextMesh : null;
            Debug.Assert(nameTextMesh != null,  nameof(nameTextMesh) + " != null");
            _loreText = _panel.Find("Lore").TryGetComponent(out TextMeshProUGUI loreTextMesh) ?  loreTextMesh : null;
            Debug.Assert(loreTextMesh != null,  nameof(loreTextMesh) + " != null");
            
            ContentChange(nameText + "/" + loreText);
            
            _panel.transform.localScale = Vector3.zero;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out IInteractionEvent interactionEvent))
                {
                    interactionEvent.InteractionTrigger = this;
                }
            }

        }

        public void ContentChange(string text)
        {
            string[] t = text.Split("/");
            
            if (_nameText == null) return;
            _nameText.text  = t[0];
            if (_loreText == null) return;
            _loreText.text = t[1];
        }

        private void Start()
        {            
            if (!saveChannelNumber) return;
            channelNumber = SaveDataManager.Instance.Data.channel;
        }

        public void ChangeChannelNumber(int number)
        {
            if (number <= channelNumber) return;
            channelNumber = number;
            
            if (!saveChannelNumber) return;
            SaveDataManager.Instance.Data.channel = channelNumber;
            SaveDataManager.Instance.Save();
        }

        public void ChangeDeadField(bool value)
        {
            dead = value;
            if (value == true)
            {
                _panel.DOScale(Vector3.zero, animationDuration).SetEase(Ease.OutCubic);
            }            
            
            else if (_enable)
            {
                _panel.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutCubic);
            }
        }

        public void SetDialogueVisible(bool value)
        {
            _enable = value;
            
            if (dead) return;
            _panel.DOKill();
            _panel.DOScale(value ? Vector3.one : Vector3.zero, animationDuration).SetEase(Ease.OutCubic);
        }        
        

        public void InteractionActive(bool value)
        {
            if (!value)
            {
                SetDialogueVisible(false);
                ChangeDeadField(true);
                return;
            }

            if (remainingIterations <= 1)
                return;

            ChangeDeadField(false);
            SetDialogueVisible(true);

            if (remainingIterations <= 150)
                remainingIterations--;
        }
        private void Update()
        {
            KeyBoardInteractionTrigger();
        }

        public void ArtificialTriggerInvoke(int channelNum)
        {
            OnInteractionTrigger?.Invoke(channelNum);
            InteractionActive(false);
        }

        private void KeyBoardInteractionTrigger()
        {
            if (_interactionLoad == null || !_enable || dead || Time.timeScale == 0) return;

            if (!Keyboard.current.eKey.isPressed)
            {
                _isInteracting = false;
                _interactionLoad.DOFillAmount(0, 0.2f).SetEase(Ease.OutBack);
                return;
            }

            if (_isInteracting) return;

            _interactionLoad.fillAmount += Time.deltaTime / interactionDuration;

            if (_interactionLoad.fillAmount < 1f) return;

            _isInteracting = true;
            _interactionLoad.fillAmount = 0f;

            OnInteractionTrigger?.Invoke(channelNumber);
            InteractionActive(false);
        }
    }
}
