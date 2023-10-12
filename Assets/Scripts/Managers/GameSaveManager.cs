using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
    private string saveFilepath;
    public static GameSaveManager Instance;
    public SaveData SaveData { get; private set; } = new SaveData();
    public bool FileExists => File.Exists(saveFilepath);
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        saveFilepath = Application.persistentDataPath + "/save";
        print(saveFilepath);
    }

    private void Update() {
        if (null != Player.Instance && Player.Instance.GameOver) {
            DeleteGame();
        }
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
        SaveData.seed = Random.Range(int.MinValue, int.MaxValue);
        string data = JsonUtility.ToJson(SaveData);
        File.WriteAllText(saveFilepath, data);
    }

    public void LoadGame() {
        if (!File.Exists(saveFilepath)) {
            NewGame();
        }
        string data = File.ReadAllText(saveFilepath);
        SaveData = JsonUtility.FromJson<SaveData>(data);
    }

    public void DeleteGame() {
        SaveData = new SaveData();
        File.Delete(saveFilepath);
    }
}