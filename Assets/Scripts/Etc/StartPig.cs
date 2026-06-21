using System;
using SaveSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Etc
{
    public class StartPig : MonoBehaviour
    {
        [SerializeField] private GameObject mePrefab;
        [SerializeField] private UnityEvent data;
        [SerializeField] private UnityEvent outData;

        [SerializeField] private bool a;
        
        private void Awake()
        {
            if (SaveDataManager.Instance.OutGameData.theEnd)
            {
                SceneManager.LoadScene("End");
            }
            
            else if (SaveDataManager.Instance.Data.ending1)
            {
                if (SceneManager.GetActiveScene().name != "InGameScene2") SceneManager.LoadScene("Ending1");
                data?.Invoke();
            }

            else if (SaveDataManager.Instance.Data.root2)
            {
                if (SceneManager.GetActiveScene().name != "InGameScene2") SceneManager.LoadScene("InGameScene2");
            }

            else if (SaveDataManager.Instance.OutGameData.ending1)
            {
                mePrefab.SetActive(true);
                outData?.Invoke();
            }
        }
    }
}