using System.Collections;
using SaveSystems;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    [DefaultExecutionOrder(-100)]
    public class StartSetting2 : MonoBehaviour
    {
        [SerializeField] private UnityEvent startSetting;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 pos;        
        [SerializeField] private Transform scooTransform;
        [SerializeField] private Vector3 scooPos;
        
        private void Start()
        {
            StartCoroutine(Starts());
        }

        private IEnumerator Starts()
        {
            yield return new WaitForSeconds(0.1f);
            if (SaveDataManager.Instance.Data.boss)
            {
                startSetting?.Invoke();
                playerTransform.position = pos;
                scooTransform.position = scooPos;
                scooTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}