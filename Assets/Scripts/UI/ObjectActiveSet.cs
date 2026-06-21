using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    [Serializable]
    public class ActiveSetAnimationSetting
    {
        public GameObject go;
        public float duration = 0.3f;
        public bool escCancel;
    }

    public class ObjectActiveSet : MonoBehaviour
    {
        [SerializeField] private ActiveSetAnimationSetting[] settings;

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) AllEscCancel();
        }
        private void AllEscCancel()
        {
            for (int i = 0; i < settings.Length; i++)
            {
                ActiveSetAnimationSetting setting = settings[i];
                if (!setting.escCancel || !setting.go.activeSelf) continue;
                
                ToggleActive(i);
                
            }
        }
        public void ToggleActive(int index)
        {
            if (index < 0 || index >= settings.Length) return;

            ActiveSetAnimationSetting setting = settings[index];
            GameObject targetGo = setting.go;
            Transform target = targetGo.transform;

            target.DOKill();

            if (targetGo.activeSelf)
            {
                target.DOScale(Vector3.zero, setting.duration)
                    .SetEase(Ease.InBack).SetUpdate(true)
                    .OnComplete(() => targetGo.SetActive(false));
            }
            else
            {
                transform.SetAsLastSibling();
                targetGo.SetActive(true);
                target.localScale = Vector3.zero;

                target.DOScale(Vector3.one, setting.duration)
                    .SetEase(Ease.OutElastic).SetUpdate(true);
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}