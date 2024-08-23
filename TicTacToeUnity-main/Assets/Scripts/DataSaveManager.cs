using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]

public class PlayerData
{
    public string winner;
    public int moveCount;
    public string time;
    public string mode;
}

public class PlayerDataList
{
    public List<PlayerData> playerDataList = new List<PlayerData>();
}

public class DataSaveManager : MonoBehaviour
{
    public PlayerDataList list = new PlayerDataList();
    PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        GenerateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateData()
    {
        playerData = new PlayerData();
        playerData.moveCount = 0;
        playerData.winner = "";
        playerData.mode = "";
        playerData.time = "";

        list.playerDataList.Add(playerData);
    }
    public void UpdateWinner(string player,int steps,bool isPVE, string time)
    {
        if (player == "Changli")
        {
            playerData.winner = "≥§¿Î";
        }
        else if (player == "Rover")
        {
            playerData.winner = "∆Ø≤¥";
        }
        else
        {
            playerData.winner = "∆Ωæ÷";
        }
        playerData.moveCount = steps;
        playerData.time = time;
        if (isPVE)
        {
            playerData.mode = "PVE";
        }
        else
        {
            playerData.mode = "PVP";
        }
        
    }

    void Clear()
    {
        GenerateData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(list, true);
        string filePath = Application.streamingAssetsPath + "/playerdatalist.json";

        using(StreamWriter sw =  new StreamWriter(filePath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
        Clear();
    }

    public PlayerDataList LoadData()
    {
        string json;
        string filePath = Application.streamingAssetsPath + "/playerdatalist.json";
        
        if (!File.Exists(filePath))
        {
            return null;
        }
        else
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            list = JsonUtility.FromJson<PlayerDataList>(json);
            return list;
        }
    }
}
