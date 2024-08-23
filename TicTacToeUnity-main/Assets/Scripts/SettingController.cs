using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{

    public static SettingController Instance;
    private static AudioSource menuAudio;
    public Slider audioSlider;
    public GameObject SettingMenu;

    public Dropdown Difficulty;
    public Dropdown PlayOrder;

    private int diff;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        menuAudio = GameObject.FindGameObjectWithTag("BGMPlayer").GetComponent<AudioSource>();
        DontDestroyOnLoad(menuAudio);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        VolumeControll();
        CloseGameSettingUI();
    }

    private void Awake()
    {

        Instance = this;
    }

    public void SetDiff()
    {
        diff = Difficulty.value;
    }
    public int GetDiff()
    {
        return diff;
    }
    public void VolumeControll()
    {
        menuAudio.volume = audioSlider.value;
    }

    //关闭设置界面
    public void CloseGameSettingUI()
    {
        //ESC
        if(Input.GetKey(KeyCode.Escape))
        {
            //游戏设置界面
            SettingMenu = GameObject.Find("SettingMenu");
            SettingMenu.SetActive(false);
        }
    }

    public void CloseSettingUI()
    {
        //游戏设置界面
        SettingMenu = GameObject.Find("SettingMenu");
        SettingMenu.SetActive(false);
        audioManager.PlayClickedAudio(audioManager.clickAudio);

    }
}
