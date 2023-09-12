using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
    public static GameSaveManager Instance;
    public SaveData SaveData { get; private set; } = new SaveData();
    private string saveFilepath;
    public bool FileExists => File.Exists(saveFilepath);
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        saveFilepath = Application.persistentDataPath + "/save";
        print(saveFilepath);
    }

    public void SaveGame() {
        SaveData.FillData();
        string data = JsonUtility.ToJson(SaveData);
        File.WriteAllText(saveFilepath, data);
    }

    public void NewGame() {
        SaveData.HP = 100;
        SaveData.maxHP = 100;
        SaveData.shield = 20;
        SaveData.maxShield = 20;
        SaveData.inventory = new Consumable[5];
        SaveData.stageCount = 1;
        SaveData.seed = (int)Random.value * 2147483647;
        string data = JsonUtility.ToJson(SaveData);
        File.WriteAllText(saveFilepath, data);
    }

    public void LoadGame() {
        if (!File.Exists(saveFilepath)) {
            Debug.LogError("���� �ҷ����� ����.");
            return;
        }
        string data = File.ReadAllText(saveFilepath);
        SaveData = JsonUtility.FromJson<SaveData>(data);
    }
}