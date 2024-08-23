using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.IO;

using Random = UnityEngine.Random;

[System.Serializable]
public class Player
{
    public Image panel;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public GameObject MainUI;
    public Text[] buttonList;
    public Text tipsText;
    public Text ChangliSteps;
    public Text RoverSteps;
    public Text timeText;
    public Text gameOverText;
    public Text recordText;
    public Text timeUsedText;

    private string playerSide;
    private bool isPVE;

    public GameObject RecordPanel;
    public GameObject gameOverPanel;
    public GameObject backButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activeColor;
    public PlayerColor inactiveColor;


    private int moveCount;
    private int changliCount;
    private int roverCount;
    private int diff;
    private int order;
    private string record;

    AudioManager audioManager;
    DataSaveManager dataSaveManager;
    TimeCounter timeCounter;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        dataSaveManager = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSaveManager>();
        timeCounter = GameObject.FindGameObjectWithTag("TimeCounter").GetComponent<TimeCounter>();

        diff = PlayerPrefs.GetInt("Difficulty");
        order= PlayerPrefs.GetInt("PlayOrder");
    }
    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    void Awake()
    {
        moveCount = 0;
        changliCount = 0;
        roverCount = 0;
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        RecordPanel.SetActive(false);
        SetPlayerColors(playerX, playerO);
    }
    public void StartPVE()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        timeCounter.StartTime();
        isPVE = true;
        MainUI.SetActive(false);
        tipsText.gameObject.SetActive(true);
        float rand = Random.Range(-1f, 1f);
        Debug.Log("随机数：：：：：：：" + rand);
        if ((rand <= 0 && order == 0) || order == 1)
        {
            playerSide = "Rover";
            //Debug.Log("漂泊者先行");
            tipsText.text = "漂泊者先行";
        }
        else
        {
            playerSide = "Changli";
            //Debug.Log("长离先行");
            tipsText.text = "长离先行";
            AiAction();
        }
    }

    public void StartPVP()
    {
        timeCounter.StartTime();
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        tipsText.gameObject.SetActive(true);
        isPVE = false;
        MainUI.SetActive(false);
        float rand = Random.Range(-1f, 1f);
        Debug.Log("随机数：：：：：：：" + rand);
        if ((rand <= 0 && order == 0) || order == 1)
        {
            playerSide = "Rover";
            //Debug.Log("漂泊者先行");
            tipsText.text = "漂泊者先行";
        }
        else
        {
            playerSide = "Changli";
            //Debug.Log("长离先行");
            tipsText.text = "长离先行";
        }
    }

    public void BackMainUI()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        //settingDataManager.LoadData();
        ChangeScene.Back();
        isPVE = false;
        RestartGame();
    }

    public void ChangeMode()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        gameOverPanel.SetActive(false);
        MainUI.SetActive(true);
        gameOverPanel.SetActive(false);
        Initialization();
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public bool GetisPVE()
    {
        return isPVE;
    }
    public void EndTurn()
    {
        moveCount++;
        if (playerSide == "Rover")
        {
            audioManager.PlayMoveAudio(audioManager.moveAudio1);
            roverCount += 1;
            RoverSteps.text = roverCount.ToString();
        }
        else
        {
            audioManager.PlayMoveAudio(audioManager.moveAudio2);
            changliCount += 1;
            ChangliSteps.text = changliCount.ToString();
        }


        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
            ChangeSide();
    }

    void ChangeSide()
    {
        playerSide = (playerSide == "Rover") ? "Changli" : "Rover";
        if (playerSide == "Rover")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            if (isPVE)
            {
                AiAction();
            }
        }
    }

    void GameOver(string winningPlayer)
    {
        timeCounter.StopTime();
        string time = timeText.text;
        if (winningPlayer == "draw")
        {
            SetGameOverText("平局!");
            dataSaveManager.UpdateWinner(winningPlayer,changliCount, isPVE, time);
        }
        else
        {
            if (winningPlayer == "Rover")
            {
                SetGameOverText("漂泊者");
                dataSaveManager.UpdateWinner(winningPlayer, roverCount, isPVE, time);
            }
            else if (winningPlayer == "Changli")
            {
                SetGameOverText("长离");
                dataSaveManager.UpdateWinner(winningPlayer, changliCount, isPVE, time);
            }
        }
        dataSaveManager.SaveData();
        SetBoardInteractable(false);
        backButton.GetComponentInParent<Button>().interactable = false;
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
        timeUsedText.text = timeText.text;
    }

    public void RestartGame()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        timeCounter.Restart();
        tipsText.gameObject.SetActive(true);
        float rand = Random.Range(-1f, 1f);
        Debug.Log("随机数：：：：：：：" + rand);
        if ((rand <= 0 && order == 0) || order == 1)
        {
            playerSide = "Rover";
            Debug.Log("漂泊者先行");
            tipsText.text = "漂泊者先行";
        }
        else
        {
            playerSide = "Changli";
            Debug.Log("长离先行");
            tipsText.text = "长离先行";
            if (isPVE)
                AiAction();
        }
        Initialization();
    }

    void Initialization()
    {
        moveCount = 0;
        changliCount = 0;
        roverCount = 0;
        ChangliSteps.text = "0";
        RoverSteps.text = "0";
        gameOverPanel.SetActive(false);
        backButton.GetComponentInParent<Button>().interactable = true;
        SetBoardInteractable(true);
        foreach (var button in buttonList)
        {
            button.text = "";
        }
        SetPlayerColors(playerX, playerO);
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i <buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activeColor.panelColor;
        //newPlayer.text.color = activeColor.textColor;
        oldPlayer.panel.color = inactiveColor.panelColor;
        //oldPlayer.text.color = inactiveColor.textColor;
    }

    async void AiAction()
    {
        foreach (var button in buttonList)
        {
            button.GetComponentInParent<GridSpace>().button.interactable = false;
        }
        await Task.Delay(TimeSpan.FromSeconds(1f));
        
        foreach (var button in buttonList)
        {
            var grid = button.GetComponentInParent<GridSpace>();
            if (string.IsNullOrEmpty(button.text))
            {
                grid.button.interactable = true;
            }
        }

        if (diff == 0)
        {
            // 1. 如果下在该位置可以赢棋，那么下在该位置
            // 2. 如果对手下在该位置可以赢棋，那下在该位置
            if (CheckAiAndPlayerWin()) return;
            // 3. 如果中心位置空闲，那么下在中心位置要优于边上和角上位置
            if (string.IsNullOrEmpty(buttonList[4].text))
            {
                buttonList[4].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("中间格子空着");
                return;
            }
            // 4. 如果角上位置空闲，那么下在角上位置要优于边上位置
            if (string.IsNullOrEmpty(buttonList[0].text))
            {
                buttonList[0].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("角上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[2].text))
            {
                buttonList[2].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("角上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[6].text))
            {
                buttonList[6].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("角上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[8].text))
            {
                buttonList[8].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("角上格子空着");
                return;
            }
            // 5. 如果只有边上位置空闲，那么只能下在边上位置
            if (string.IsNullOrEmpty(buttonList[1].text))
            {
                buttonList[1].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("边上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[3].text))
            {
                buttonList[3].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("边上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[5].text))
            {
                buttonList[5].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("边上格子空着");
                return;
            }
            if (string.IsNullOrEmpty(buttonList[7].text))
            {
                buttonList[7].GetComponentInParent<GridSpace>().SetSpace();
                Debug.Log("边上格子空着");
            }
        }
        else
        {
            for (int i = 0; i < buttonList.Length; i++)
            {
                if (string.IsNullOrEmpty(buttonList[i].text))
                {
                    buttonList[i].GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("顺序寻找空位");
                    return;
                }
            }
        }
        //audioManager.PlayMoveAudio(audioManager.moveAudio1);
    }

    private bool CheckAiAndPlayerWin()
    {
        Text canWinGrid = null;
        Text canLoseGrid = null;
       
        int winCount = 0;
        int loseCount = 0;
        for (int i = 0; i < 3; i++) 
        {
            //检查行
            winCount = 0;
            loseCount = 0;
            switch (buttonList[3 * i].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i];
                    canLoseGrid = buttonList[3 * i];
                    break;
            }
            switch (buttonList[3 * i + 1].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i + 1];
                    canLoseGrid = buttonList[3 * i + 1];
                    break;
            }
            switch (buttonList[3 * i + 2].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i + 2];
                    canLoseGrid = buttonList[3 * i + 2];
                    break;
            }

            if (winCount + loseCount < 3)
            {
                if (winCount == 2)
                {
                    if (canWinGrid != null)
                    {
                        canWinGrid.GetComponentInParent<GridSpace>().SetSpace(); 
                        Debug.Log("Ai能在" +(i+1) + "行赢");
                    }
                    return true;
                }
                if (loseCount == 2)
                {
                    if (canLoseGrid != null)
                    {
                        canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();   
                        Debug.Log("Player能在" + (i+1) + "行赢");
                    }
                    return true;
                }
            }
            
            //检查列
            winCount = 0;
            loseCount = 0;
            switch (buttonList[i].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i];
                    canLoseGrid = buttonList[i];
                    break;
            }
            switch (buttonList[i + 3].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i + 3];
                    canLoseGrid = buttonList[i + 3];
                    break; 
            }
            switch (buttonList[i + 6].text)
            {
                case "Rover":
                    loseCount++;
                    break;
                case "Changli":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i + 6];
                    canLoseGrid = buttonList[i + 6];
                    break;
            }

            if (winCount + loseCount < 3)
            {
                if (winCount == 2)
                {
                    if (canWinGrid != null)
                    {
                        canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                        Debug.Log("Ai能在" + (i + 1) + "列赢");
                    }

                    return true;
                }

                if (loseCount == 2)
                {
                    if (canLoseGrid != null)
                    {
                        canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                        Debug.Log("Player能在" + (i + 1) + "列赢");
                    }

                    return true;
                }
            }
        }
        
        //检查对角线1
        winCount = 0;
        loseCount = 0;
        switch (buttonList[0].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[0];
                canLoseGrid = buttonList[0];
                break;
        }
        switch (buttonList[4].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[4];
                canLoseGrid = buttonList[4]; 
                break;
        }
        switch (buttonList[8].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[8];
                canLoseGrid = buttonList[8];
                break;
        }

        if (winCount + loseCount < 3)
        {
            if (winCount == 2)
            {
                if (canWinGrid != null)
                {
                    canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Ai能在对角线1赢");
                }

                return true;
            }

            if (loseCount == 2)
            {
                if (canLoseGrid != null)
                {
                    canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Player能在对角线1赢");
                }

                return true;
            }
        }

        //检查对角线2
        winCount = 0;
        loseCount = 0;
        switch (buttonList[2].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[2];
                canLoseGrid = buttonList[2];
                break;
        }
        switch (buttonList[4].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[4];
                canLoseGrid = buttonList[4];
                break;
        }
        switch (buttonList[6].text)
        {
            case "Rover":
                loseCount++;
                break;
            case "Changli":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[6];
                canLoseGrid = buttonList[6];
                break;
        }

        if (winCount + loseCount < 3)
        {
            if (winCount == 2)
            {
                if (canWinGrid != null)
                {
                    canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Ai能在对角线2赢");
                }

                return true;
            }

            if (loseCount == 2)
            {
                if (canLoseGrid != null)
                {
                    canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Player能在对角线2赢");
                }

                return true;
            }
        }

        return false;
    }

    public void ReadGameRecord()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
        RecordPanel.SetActive(true);
        PlayerDataList list = dataSaveManager.LoadData();
        foreach (PlayerData data in list.playerDataList)
        {
            string winner = data.winner;
            int moveCount = data.moveCount;
            string time = data.time;
            string mode = data.mode;
            record += "  " + winner + "          " + moveCount + "       " + time + "        " + mode + "\n";
        }
        recordText.text = record;
        record = "";
    }

    public void CloseRecordPanel()
    {
        RecordPanel.SetActive(false);
        audioManager.PlayClickedAudio(audioManager.clickAudio1);
    }
}

