using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    void Start()
    {
        SceneMgr.GetInstance().SwitchingScene(SceneType.SceneLogin);
        NetWriter.SetUrl("127.0.0.1:9001");
    }
}
