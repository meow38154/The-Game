using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SaveSystems
{
    [Serializable]
    public class SaveData
    {
        public bool pigDialogue;
        public int channel;
        public bool pigEnd;

        public bool ending1;
    }

    public class SaveDataManager : MonoBehaviour
    {
        public static SaveDataManager Instance { get; private set; }

        public SaveData Data { get; private set; }

        private string SavePath =>
            Path.Combine(Application.dataPath, "NPCDatas.json");

        public event Action OnSave;
        public event Action OnLoad;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Load();
        }

        private void Update()
        {
            if (Keyboard.current.lKey.isPressed && Keyboard.current.sKey.isPressed &&
                Keyboard.current.tKey.wasPressedThisFrame)
            {
                Debug.Log("데이터가 리셋되었습니다. 빌드를 재시작 하세요.");
                Data.pigDialogue = false;
                Data.pigEnd = false;
                Data.channel = 0;
                Save();
            }
        }

        public void SetValue(string fieldName, object value)
        {
            FieldInfo field = typeof(SaveData).GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError($"Field not found : {fieldName}");
                return;
            }

            field.SetValue(Data, Convert.ChangeType(value, field.FieldType));

            Save();
        }

        public T GetValue<T>(string fieldName)
        {
            FieldInfo field = typeof(SaveData).GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError($"Field not found : {fieldName}");
                return default;
            }

            return (T)field.GetValue(Data);
        }

        public void Save()
        {
            OnSave?.Invoke();
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(SavePath, json);
        }

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                Data = new SaveData();
                Save();
                return;
            }
            OnLoad?.Invoke();
            string json = File.ReadAllText(SavePath);
            Data = JsonUtility.FromJson<SaveData>(json);
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            Data = new SaveData();
        }
    }
}