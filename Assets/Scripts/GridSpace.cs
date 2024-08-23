using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public Sprite RoverSprite;
    public Sprite ChangliSprite;
    public Image PlayerImg;
    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        this.gameController = controller;
    }

    public void SetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        gameController.EndTurn();
    }

    private void Update()
    {
        if (buttonText.text == "Rover")
        {
            PlayerImg.sprite = RoverSprite;
            button.interactable = false;
        }
        else if (buttonText.text == "Changli")
        {
            PlayerImg.sprite = ChangliSprite;
            button.interactable = false;
        } 
        else if (buttonText.text == "")
        {
            PlayerImg.sprite = null;
        }
    }
}
