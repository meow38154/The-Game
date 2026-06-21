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
        public bool root2;
        public bool boss;
    }

    [Serializable]
    public class OutGameSaveData
    {
        public bool ending1;
        public bool theEnd;
    }

    [DefaultExecutionOrder(-10)]
    public class SaveDataManager : MonoBehaviour
    {
        public static SaveDataManager Instance { get; private set; }

        public SaveData Data { get; private set; }
        public OutGameSaveData OutGameData { get; private set; }

        private string SavePath =>
            Path.Combine(Application.dataPath, "NPCDatas.json");

        private string OutGameSavePath =>
            Path.Combine(Application.persistentDataPath, "OutGameNPCDatas.json");

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
            LoadOutGame();
        }

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.lKey.isPressed &&
                Keyboard.current.sKey.isPressed &&
                Keyboard.current.tKey.wasPressedThisFrame)
            {
                Debug.Log("게임 내 데이터가 리셋되었습니다. 빌드를 재시작 하세요.");

                Data.pigDialogue = false;
                Data.pigEnd = false;
                Data.channel = 0;
                Data.ending1 = false;
                Data.root2 = false;
                OutGameData.ending1 = false;

                Save();
            }
        }

        public void SetValue(string fieldName, object value)
        {
            SetValueInternal(Data, fieldName, value);
            Save();
        }

        public T GetValue<T>(string fieldName)
        {
            return GetValueInternal<T>(Data, fieldName);
        }

        public void SetOutGameValue(string fieldName, object value)
        {
            SetValueInternal(OutGameData, fieldName, value);
            SaveOutGame();
        }

        public T GetOutGameValue<T>(string fieldName)
        {
            return GetValueInternal<T>(OutGameData, fieldName);
        }

        private static void SetValueInternal<TData>(TData data, string fieldName, object value)
        {
            FieldInfo field = typeof(TData).GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError($"Field not found : {fieldName}");
                return;
            }

            field.SetValue(data, Convert.ChangeType(value, field.FieldType));
        }

        private static TValue GetValueInternal<TValue, TData>(TData data, string fieldName)
        {
            FieldInfo field = typeof(TData).GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError($"Field not found : {fieldName}");
                return default;
            }

            return (TValue)field.GetValue(data);
        }

        private static TValue GetValueInternal<TValue>(object data, string fieldName)
        {
            FieldInfo field = data.GetType().GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError($"Field not found : {fieldName}");
                return default;
            }

            return (TValue)field.GetValue(data);
        }

        public void Save()
        {
            OnSave?.Invoke();

            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(SavePath, json);
        }

        public void SaveOutGame()
        {
            string json = JsonUtility.ToJson(OutGameData, true);
            File.WriteAllText(OutGameSavePath, json);
        }

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                Data = new SaveData();
                Save();
                return;
            }

            string json = File.ReadAllText(SavePath);
            Data = JsonUtility.FromJson<SaveData>(json);

            OnLoad?.Invoke();
        }

        public void LoadOutGame()
        {
            if (!File.Exists(OutGameSavePath))
            {
                OutGameData = new OutGameSaveData();
                SaveOutGame();
                return;
            }

            string json = File.ReadAllText(OutGameSavePath);
            OutGameData = JsonUtility.FromJson<OutGameSaveData>(json);
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            Data = new SaveData();
            Save();
        }

        public void DeleteOutGameSave()
        {
            if (File.Exists(OutGameSavePath))
                File.Delete(OutGameSavePath);

            OutGameData = new OutGameSaveData();
            SaveOutGame();
        }

        public void DeleteAllSave()
        {
            DeleteSave();
            DeleteOutGameSave();
        }
    }
}