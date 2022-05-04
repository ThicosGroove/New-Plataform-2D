using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSavingData : MonoBehaviour
{
    public static GameSavingData Instance { get; private set; }

    public string _name;
    public int _level;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadNewData();
    }

    [System.Serializable]
    public class SaveData
    {
        public string Name;
        public int Level;
    }

    public void SaveNewData()
    {
        SaveData data = new SaveData();

        data.Name = _name;
        data.Level = _level;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveDataPlayer.json", json);
    }

    public void LoadNewData()
    {
        string path = Application.persistentDataPath + "/saveDataPlayer.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            _name = data.Name;
            _level = data.Level;
        }
    }
}
