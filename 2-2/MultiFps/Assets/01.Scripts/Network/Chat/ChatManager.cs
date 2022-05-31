using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;
   
    #region SERVER
    public Button btnServerStart;
    public Button btnServerClose;
    public InputField inputServerPort;
    public Text txtServerStatus;
    public Text txtServerLocalIP;
    #endregion

    #region CLIENT
    public Button btnClientConnect;
    public Button btnClientDisconnect;
    public InputField inputIP;
    public InputField inputPort;
    public InputField inputNickname;
    public Text txtClientStatus;
    public VerticalLayoutGroup userListContentView;
    public Text userCellPref;
    //      List<Text> textList = Text
    private List<int> userTextIndexList = new List<int>(); // [ID]
    public Button[] btnsGameStart;
    #endregion

    #region CHAT
    public InputField inputChat;
    public VerticalLayoutGroup contentView;
    public Text chatPref;
    #endregion


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        userTextIndexList.Add(-1);  
        inputChat.onEndEdit.AddListener(_ =>
        {
            SendChat();
        });

        if(NetServer.Instance.GetServerStarted())
        {
            OnChangeServerStatus(true);
        }
        if(NetClient.Instance.GetConnected())
        {
            OnChangeClientStatus(true);
        }
    }

    public void StartServer()
    {
        try
        {
            int port = int.Parse(inputServerPort.text);
            NetServer.Instance.InitializeServer(port);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public void SetLocalIP(string ip)
    {
        txtServerLocalIP.text = ip;
    }

    public void ConnectToServer()
    {
        string ip = inputIP.text;
        string port = inputPort.text;
        string nickName = inputNickname.text;

        if(nickName == "")
        {
            Debug.Log("이름을 입력해 주세요.");
            return;
        }

        NetClient.Instance.ConnectToServer(ip, port, nickName);

    }

    public void ResetUI()
    {

        Text[] names = userListContentView.transform.GetComponentsInChildren<Text>();

        for (int i = 0; i < names.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }

            Destroy(names[i].gameObject);
        }

        Text[] chats = contentView.transform.GetComponentsInChildren<Text>();

        for (int i = 0; i < chats.Length; i++)
        {
            Destroy(chats[i].gameObject);
        }

        foreach(var item in btnsGameStart)
        {
            item.interactable = false;
        }

    }

    public void SendChat()
    {
        string chat = inputChat.text;

        if (chat == "") return;

        NetClient.Instance.SendChat(chat);

        inputChat.text = "";
    }

    //public void OnGetClientList(string[] clientNames)
    //{
    //    foreach (string userName in clientNames)
    //    {
    //        AddUserToList(userName);
    //    }
    //}

    public void OnUserJoin(int clientId, string userName)
    {

        Text newChat = Instantiate(chatPref, contentView.transform);
        newChat.text = $"{userName}님이 입장하셨습니다!";

        AddUserToList(clientId, userName);
    }

    public void InteractableStartButton()
    {
        foreach (var item in btnsGameStart)
        {
            item.interactable = true;
        }
    }


    private void AddUserToList(int clientId, string userName)
    {
        Text clientName = Instantiate(userCellPref, userListContentView.transform);
        
        userTextIndexList.Add(clientId);

        clientName.text = $"{userName}";
    }

    // name => id
    public void OnUserDisconnect(int clientIdx, string userName)
    {
        //TODO: 1. Text 지우기, 2. userTextIndexList.Item 지우기

        int textIndex = userTextIndexList.FindIndex(clientIndex => clientIndex == clientIdx);
        Transform userTextTransform = userListContentView.transform.GetChild(textIndex);

        Destroy(userTextTransform.gameObject);
        userTextIndexList.RemoveAt(textIndex);

        //Text[] names = userListContentView.transform.GetComponentsInChildren<Text>();

        //for (int i = 1; i < names.Length; i++)
        //{
        //    if (names[i].text == userName)
        //    {
        //        Destroy(names[i].gameObject);
        //        break;
        //    }
        //}


        Text newChat = Instantiate(chatPref, contentView.transform);
        newChat.text = $"{userName}님이 나갔습니다!";
    }

    public void OnReadChat(string name,string chat)
    {
        Text newChat = Instantiate(chatPref, contentView.transform);
        newChat.text = $"{name} : {chat}";
    }

    public void OnChangeServerStatus(bool serverStarted)
    {
        btnServerStart.interactable = !serverStarted;
        btnServerClose.interactable = serverStarted;

        string status = serverStarted ? "OPEN" : "CLOSED";
        txtServerStatus.text = $"STATUS : {status}";
    }

    public void OnChangeClientStatus(bool clientConnected)
    {
        if(btnClientConnect != null)
        {
            btnClientConnect.interactable = !clientConnected;
            btnClientDisconnect.interactable = clientConnected;

            string status = clientConnected ? "CONNECTED" : "DISCONNECTED";
            txtClientStatus.text = $"STATUS : {status}";
        }


    }

    // gameType : 0 = MultiPlay, 1 = CoOpPlay
    public void HandleStartGame(int gameType)
    {
        NetClient.Instance.SendStartGame(gameType);

        //NetClient.Instance.StartScene(NetClient.Scene.MultiplayScene);
    }


    public void CloseServer()
    {
        NetServer.Instance.CloseServer();
    }
    public void DisconnectToServer()
    {
        NetClient.Instance.DisconnectToServer();
    }
}
