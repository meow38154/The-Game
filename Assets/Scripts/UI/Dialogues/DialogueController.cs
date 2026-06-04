using System;
using System.Collections;
using Agents.Players;
using DG.Tweening;
using GGMLib.ModuleSystem;
using TMPro;
using UI.Interaction.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility;

namespace UI.Dialogues
{
    public class DialogueController : MonoBehaviour
    {
        [Header("Dialogue Settings")]
        [SerializeField] private float textPrintSpeed = 0.05f;

        [Header("Requires allocation")]
        [SerializeField] private Transform dialoguePanel;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI contentText;

        private Coroutine _dialogueRoutine;
        private Tween _textTween;
        private IControlMovement _controlMovement;

        private GameObject _player;
        private IInteractionTrigger _actionInteractionObject;

        private void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void Start()
        {
            if (_player == null) return;

            _controlMovement = _player
                .GetComponent<ModuleOwner>()
                .GetModule<IControlMovement>();
        }

        private void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            _player = message.gameObject;
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
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);

            if (_dialogueRoutine != null)
                StopCoroutine(_dialogueRoutine);

            _textTween?.Kill();
        }

        private void DialogueRenderPlay(DialogueStartMessage message)
        {
            if (_dialogueRoutine != null)
                StopCoroutine(_dialogueRoutine);

            _actionInteractionObject = message.interactionObject;
            _dialogueRoutine = StartCoroutine(DialogueRender(message.dialogues, message.unityEvent));
        }

        private IEnumerator DialogueRender(DialogueBundleData[] dialogues, UnityEvent unityEvent)
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
                }
            }

            if (_controlMovement != null)
                _controlMovement.CanManualMovement = true;

            dialoguePanel.gameObject.SetActive(false);

            if (_actionInteractionObject != null)
                _actionInteractionObject.InteractionActive(true);

            unityEvent?.Invoke();
            
            _dialogueRoutine = null;
        }

        private IEnumerator PrintText(string content, Action<bool> onComplete)
        {
            bool skipped = false;

            content = content.Replace("{C}", Environment.UserName);
            content = content.Replace("{D}", Environment.UserDomainName);

            contentText.text = "";

            _textTween?.Kill();

            float duration = content.Length * textPrintSpeed;

            _textTween = contentText
                .DOText(content, duration)
                .SetEase(Ease.Linear);

            while (_textTween != null && _textTween.IsActive() && _textTween.IsPlaying())
            {
                if (IsEnterPressed())
                {
                    skipped = true;
                    _textTween.Complete();
                    break;
                }

                yield return null;
            }

            onComplete?.Invoke(skipped);
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