using System.Collections;
using Agents.Players;
using DG.Tweening;
using GGMLib.ModuleSystem;
using TMPro;
using UI.Interaction.Interface;
using UnityEngine;
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
            _controlMovement = _player.GetComponent<ModuleOwner>().GetModule<IControlMovement>();
        }

        private void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            _player = message.gameObject;            
        }

        private void OnEnable()
        {
            EventBus.Subscribe<DialogueStartMessage>(DialogueRenderPlay);
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

            _dialogueRoutine = StartCoroutine(DialogueRender(message.dialogues));
            _actionInteractionObject = message.interactionObject;
        }

        
        private IEnumerator DialogueRender(DialogueBundleData[] dialogues)
        {
            _controlMovement.CanManualMovement = false;
            
            dialoguePanel.gameObject.SetActive(true);

            foreach (var dialogue in dialogues)
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
                    
                    contentText.text = "";

                    _textTween?.Kill();

                    float duration = content.Length * textPrintSpeed;
                    _textTween = contentText.DOText(content, duration)
                        .SetEase(Ease.Linear);

                    yield return _textTween.WaitForCompletion();
                    
                    yield return WaitEnterUp();
                    yield return WaitEnterDown();
                }
            }
            _controlMovement.CanManualMovement = true;
            
            dialoguePanel.gameObject.SetActive(false);
            
            _actionInteractionObject.InteractionActive(true);
            
            _dialogueRoutine = null;
        }

        private static bool IsEnterPressed()
        {    if (Time.timeScale == 0f)
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