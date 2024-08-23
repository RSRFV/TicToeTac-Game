using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayerController : MonoBehaviour
{
    private static BGMPlayerController instance;

    private void Awake()
    {
        //ȷ��ֻ��һ��BGMPlayerʵ���ڿ糡��ʱ����
        instance = this;
        if (FindObjectsOfType<BGMPlayerController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        //��Ǹ���Ϸ�����ڿ糡��ʱ��������
        DontDestroyOnLoad(gameObject);
    }
}
