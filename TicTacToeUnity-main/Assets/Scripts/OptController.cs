using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptController : MonoBehaviour
{
    private GameObject SettingMenu;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        SettingMenu = GameObject.Find("SettingMenu");
        SettingMenu.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio);
        audioManager.PlayStartAudio();
        ChangeScene.Jump();
    }

    public void SettingGame()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio);
        //GameObject settingMenu = GameObject.FindGameObjectWithTag("SettingMenu").gameObject;
        SettingMenu.SetActive(SettingMenu.activeSelf == true ? false : true);
    }

    public void OnExitGame()
    {
        audioManager.PlayClickedAudio(audioManager.clickAudio);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
