using System.Collections;
using DG.Tweening;
using SaveSystems;
using UI.Dialogues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.FakeEnding
{
    public class FakeEnding : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;
        [SerializeField] private DialogueGroupDataSo groupData;
        
        public UnityEvent OnDialogueEnd;
        
        private void OnEnable()
        {
            FadeBackground();
            MoveText();
            StartCoroutine(Dialogue());
        }

        private IEnumerator Dialogue()
        {
            yield return new WaitForSeconds(7.8f);
            DialoguePlay();
        }

        private void DialoguePlay()
        {
            if (!SaveDataManager.Instance.Data.pigDialogue) return;
            if (groupData == null) return;
            
            EventBus.Publish(new DialogueStartMessage(groupData.Dialogues[0].DialogueBundleData, null, new UnityEvent(), null, 1));
            SaveDataManager.Instance.Data.pigEnd = true;
            SaveDataManager.Instance.Save();
            OnDialogueEnd?.Invoke();
        }

        private void FadeBackground()
        {
            image.DOFade(1, 5f).SetEase(Ease.Linear);
        }

        private void MoveText()
        {
            rectTransform.DOAnchorPosY(1500, 30).SetEase(Ease.Linear);
        }
    }
}