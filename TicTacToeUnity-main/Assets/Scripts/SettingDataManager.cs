using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SettingData
{
    public int difficulty;
    public int playorder;
}

public class SettingDataList
{
    public List<SettingData> settingDataList = new List<SettingData>();
}

public class SettingDataManager : MonoBehaviour
{
    public SettingDataList list = new SettingDataList();
    public Dropdown Difficulty;
    public Dropdown PlayOrder;

    SettingData settingData;
    // Start is called before the first frame update
    void Start()
    {
        GenerateData();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateData()
    {
        settingData = new SettingData();
        settingData.difficulty = 0;
        settingData.playorder = 0;
        list.settingDataList.Add(settingData);
    }

    public void SetData()
    {
        settingData.difficulty = Difficulty.value;
        settingData.playorder = PlayOrder.value;
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(list,true);
        string filePath = Application.streamingAssetsPath + "/settingData.json";

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
        PlayerPrefs.SetInt("Difficulty", Difficulty.value);
        PlayerPrefs.SetInt("PlayOrder", PlayOrder.value);
    }

    public void LoadData()
    {
        string json;
        string filePath = Application.streamingAssetsPath + "/settingData.json";
        if (!File.Exists(filePath))
        {
            return;
        }
        else
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            list = JsonUtility.FromJson<SettingDataList>(json);
            settingData = list.settingDataList[0];
            int diff = settingData.difficulty;
            int order = settingData.playorder;
            Difficulty.value = diff;
            PlayOrder.value = order;
        }
    }
}
