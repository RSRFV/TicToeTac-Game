using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayerController : MonoBehaviour
{
    private static BGMPlayerController instance;

    private void Awake()
    {
        //确保只有一个BGMPlayer实例在跨场景时存在
        instance = this;
        if (FindObjectsOfType<BGMPlayerController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        //标记该游戏对象在跨场景时不被销毁
        DontDestroyOnLoad(gameObject);
    }
}
