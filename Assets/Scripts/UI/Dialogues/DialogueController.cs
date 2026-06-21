using System;
using System.Collections;
using Agents.Players;
using DG.Tweening;
using GGMLib.ModuleSystem;
using Sound;
using TMPro;
using UI.Interaction.Events;
using UI.Interaction.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility;

namespace UI.Dialogues
{
    public class DialogueController : MonoBehaviour, IGetPlayer
    {
        [Header("Dialogue Settings")]
        [SerializeField] private float textPrintSpeed = 0.05f;

        [Header("Requires allocation")]
        [SerializeField] private Transform dialoguePanel;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI contentText;

        [Header("Sound")]
        [SerializeField] private SoundClipSO diaRenderSound;
        [SerializeField] private SoundClipSO nextDiaSound;

        private Coroutine _dialogueRoutine;
        private Tween _textTween;
        private IControlMovement _controlMovement;

        public GameObject Player { get; set; }

        private IInteractionTrigger _actionInteractionObject;

        private void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void Start()
        {
            TryCacheControlMovement();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<DialogueStartMessage>(DialogueRenderPlay);

            if (dialoguePanel != null)
                dialoguePanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<DialogueStartMessage>(DialogueRenderPlay);

            if (_dialogueRoutine != null)
            {
                StopCoroutine(_dialogueRoutine);
                _dialogueRoutine = null;
            }

            _textTween?.Kill();

            if (_controlMovement != null)
                _controlMovement.CanManualMovement = true;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
            TryCacheControlMovement();
        }

        private void TryCacheControlMovement()
        {
            if (Player == null)
                return;

            ModuleOwner moduleOwner = Player.GetComponent<ModuleOwner>();
            if (moduleOwner == null)
                return;

            _controlMovement = moduleOwner.GetModule<IControlMovement>();
        }

        private void DialogueRenderPlay(DialogueStartMessage message)
        {
            if (_dialogueRoutine != null)
                StopCoroutine(_dialogueRoutine);

            _actionInteractionObject = message.interactionObject;
            _dialogueRoutine = StartCoroutine(DialogueRender(
                message.dialogues,
                message.unityEvent,
                message.addQuestEvent,
                message.channelNum
            ));
        }

        private IEnumerator DialogueRender(
            DialogueBundleData[] dialogues,
            UnityEvent unityEvent,
            AddQuestEvent[] addQuestEvent,
            int channelNum)
        {
            if (_controlMovement != null)
                _controlMovement.CanManualMovement = false;

            dialoguePanel.gameObject.SetActive(true);

            foreach (DialogueBundleData dialogue in dialogues)
            {
                if (dialogue.profile != null)
                {
                    image.sprite = dialogue.profile.ProfileIcon;
                    nameText.text = dialogue.profile.Name;
                }
                else
                {
                    image.sprite = null;
                    nameText.text = "";
                }

                foreach (string content in dialogue.dialogues)
                {
                    yield return WaitEnterUp();

                    bool skipped = false;
                    yield return PrintText(content, value => skipped = value);

                    if (skipped)
                        yield return WaitEnterUp();

                    yield return WaitEnterDown();
                    PlaySound(nextDiaSound);

                    _textTween?.Kill(false);
                    contentText.text = "";
                    contentText.ForceMeshUpdate();

                    yield return null;
                }
            }

            if (_controlMovement != null)
                _controlMovement.CanManualMovement = true;

            dialoguePanel.gameObject.SetActive(false);

            if (_actionInteractionObject != null)
                _actionInteractionObject.InteractionActive(true);

            if (addQuestEvent != null)
            {
                foreach (AddQuestEvent questEvent in addQuestEvent)
                {
                    if (questEvent.InteractionTrigger.ChannelNumber == questEvent.ChannelNumber)
                    {
                        questEvent.AddQuest();
                        questEvent.End = true;
                    }
                }
            }

            unityEvent?.Invoke();

            _dialogueRoutine = null;
        }

        private IEnumerator PrintText(string content, Action<bool> onComplete)
        {
            bool skipped = false;

            content = content.Replace("{C}", Environment.UserName);
            content = content.Replace("{D}", Environment.UserDomainName);

            _textTween?.Kill(true);

            contentText.text = "";
            contentText.ForceMeshUpdate();

            yield return null;

            float duration = content.Length * textPrintSpeed;
            int lastTextLength = 0;

            _textTween = contentText
                .DOText(content, duration)
                .SetEase(Ease.Linear);

            while (_textTween != null && _textTween.IsActive() && _textTween.IsPlaying())
            {
                int currentTextLength = contentText.text.Length;

                if (currentTextLength > lastTextLength)
                {
                    PlaySound(diaRenderSound);
                    lastTextLength = currentTextLength;
                }

                if (IsEnterPressed())
                {
                    skipped = true;
                    PlaySound(nextDiaSound);
                    _textTween.Complete();
                    break;
                }

                yield return null;
            }

            contentText.text = content;
            contentText.ForceMeshUpdate();

            onComplete?.Invoke(skipped);
        }

        private void PlaySound(SoundClipSO sound)
        {
            if (sound == null)
                return;

            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
        }

        private static bool IsEnterPressed()
        {
            if (Time.timeScale == 0f)
                return false;

            return Keyboard.current != null &&
                   (Keyboard.current.enterKey.wasPressedThisFrame ||
                    Keyboard.current.numpadEnterKey.wasPressedThisFrame);
        }

        private static IEnumerator WaitEnterDown()
        {
            yield return new WaitUntil(() =>
                Time.timeScale > 0f &&
                Keyboard.current != null &&
                (Keyboard.current.enterKey.wasPressedThisFrame ||
                 Keyboard.current.numpadEnterKey.wasPressedThisFrame));
        }

        private static IEnumerator WaitEnterUp()
        {
            yield return new WaitUntil(() =>
                Keyboard.current == null ||
                (!Keyboard.current.enterKey.isPressed &&
                 !Keyboard.current.numpadEnterKey.isPressed));
        }
    }
}