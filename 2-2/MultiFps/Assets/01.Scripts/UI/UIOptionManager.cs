using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOptionManager : MonoBehaviour
{

    public void HandleRestart()
    {
        Time.timeScale = 1;
        NetClient.Instance.StartScene(NetClient.Scene.MultiplayScene);
    }

    public void HandleExit()
    {
        //Application.Quit();
        NetClient.Instance.StartScene(NetClient.Scene.SocketChatTutorialScene);
    }
}
