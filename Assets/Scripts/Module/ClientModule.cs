using MLAPI;
using MLAPI.Transports.UNET;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientModule : MonoBehaviour
{
    public static ClientModule Singleton { get; protected set; }

    public enum UIType
    {
        NORMAL,
        PLAY
    };
    public enum PlayStatus
    {
        LOSE,
        PLAY,
        WIN,
        MOVE
    };

    public class ConnectionInfo
    {
        public string Addess = "127.0.0.1";
        public int Port = 7777;
        public string WorldId = "Map001";

        public string PlayerSessionId = "";
        public string TicketId = "";
        public bool MatchSuccess = false;
        public int PollingCount = 0;

        public void Init()
        {
            this.Addess = "127.0.0.1";
            this.Port = 7777;
            this.WorldId = "Map001";
            this.PlayerSessionId = "";
            this.TicketId = "";
            this.MatchSuccess = false;
            this.PollingCount = 0;
        }
    }
    public string PlayerId = "";

    public ConnectionInfo ClientConnection = new ConnectionInfo();

    public bool LocalTest;

    public PlayStatus PlayerStatus = PlayStatus.PLAY;

    public APIModule ApiModule = new APIModule();

    void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogModule.WriteToLogFile("[ClientModule] On SceneLoad - " + scene.name);
        // If scene is about game, connect to game server
        ConnectToServer();
    }


    #region UI Handle

    [SerializeField]
    public InputField NameInputField;
    [SerializeField]
    public InputField PasswordInputField;
    public Button SignInBtn;
    public Button SignUpBtn;
    public Text ConnectionStatus;

    public void OnGUI()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        GUI.contentColor = Color.black;
        if (activeSceneName != "Login")
        {
            GUI.Label(new Rect(10, 10, 200, 30), "World Name : " + activeSceneName);
            string message = "";
            if (PlayerStatus == PlayStatus.LOSE)
            {
                message = "Status : Game Over";
            }
            else if (PlayerStatus == PlayStatus.PLAY)
            {
                message = "Status : Player Alive";
            }
            else if (PlayerStatus == PlayStatus.WIN)
            {
                message = "Status : Player Win";
            }
            else if (PlayerStatus == PlayStatus.MOVE)
            {
                message = "Status : Move World";
            }
            GUI.Label(new Rect(10, 50, 200, 30), message);
        }
        if (PlayerStatus == PlayStatus.WIN)
        {
            GUI.Label(new Rect(10, 90, 200, 30), "Server will Restart soon!");
        }
    }

    public void SetMainUI(UIType uIType)
    {
        if (uIType == UIType.NORMAL)
        {
            //Init
            if (NameInputField != null)
            {
                NameInputField.gameObject.SetActive(true);
            }
            if (PasswordInputField != null)
            {
                PasswordInputField.gameObject.SetActive(true);
            }
            if (SignInBtn != null)
            {
                SignInBtn.gameObject.SetActive(true);
            }
            if (SignUpBtn != null)
            {
                SignUpBtn.gameObject.SetActive(true);
            }
            if (ConnectionStatus != null)
            {
                ConnectionStatus.gameObject.SetActive(true);
            }
        }
        else if (uIType == UIType.PLAY)
        {
            //Hosting
            if (NameInputField != null)
            {
                NameInputField.gameObject.SetActive(false);
            }
            if (PasswordInputField != null)
            {
                PasswordInputField.gameObject.SetActive(false);
            }
            if (SignInBtn != null)
            {
                SignInBtn.gameObject.SetActive(false);
            }
            if (SignUpBtn != null)
            {
                SignUpBtn.gameObject.SetActive(false);
            }
            if (ConnectionStatus != null)
            {
                ConnectionStatus.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickSignInBtn()
    {
        string inputName = NameInputField.text;
        string inputPassword = PasswordInputField.text;

        LogModule.WriteToLogFile("[ClientModule] Onclick - " + ClientModule.Singleton);
        if (ConnectionStatus != null)
        {
            ConnectionStatus.text = "Entering the world";
        }

        if (ClientModule.Singleton.SignIn(inputName, inputPassword))
        {
            // Login existing server
            ClientModule.Singleton.SignInProcess(inputName);
        }
        else
        {
            LogModule.WriteToLogFile("[ClientModule] Sign In Failed. inputName-" + inputName + ", inputPassword-" + inputPassword);
        }
    }

    public void OnClickSignUpBtn()
    {
        string inputName = NameInputField.text;
        string inputPassword = PasswordInputField.text;

        if (ClientModule.Singleton.SignUp(inputName, inputPassword))
        {
            LogModule.WriteToLogFile("[ClientModule] Sign Up Succeeded!");
        }
        else
        {
            LogModule.WriteToLogFile("[ClientModule] Sign Up Failed. inputName-" + inputName + ", inputPassword-" + inputPassword);
        }
    }
    #endregion


    #region Auth Process
    public bool SignIn(string inputName, string inputPassword)
    {
        // Implement SignIn Validation
        return true;
    }

    public void SignInProcess(string inputName)
    {
        LogModule.WriteToLogFile("[ClientModule] Sign In Process");
        PlayerId = inputName;

#if UNITY_EDITOR
        if (LocalTest)
        {
            Debug.Log("Local Test Sign In");
            SetMainUI(UIType.PLAY);
            this.ClientConnection.Addess = "127.0.0.1";
            this.ClientConnection.Port = 7777;
            this.ClientConnection.PlayerSessionId = "Player!";
            DisconnectToServer();
            SceneManager.LoadScene(this.ClientConnection.WorldId);
        }
        else
        {
            MoveToWorld("Map001");
        }
#else
        MoveToWorld("Map001");
#endif
    }

    public bool SignUp(string inputName, string inputPassword)
    {
        return true;
    }
#endregion

#region ClientNetwork
    public void MoveToWorld(string worldId)
    {
        if (ConnectionStatus != null)
        {
            ConnectionStatus.text = "Searching World . . .";
        }
        this.ClientConnection.Init();
        this.ClientConnection.WorldId = worldId;
        StartCoroutine(HttpModule.PutRequest(ApiModule.GetMatchRequestAPI(),
            new APIModule.MatchmakingRequest(this.PlayerId, worldId), MatchRequestCallback));
    }

    public void ConnectToServer()
    {
        LogModule.WriteToLogFile("[ClientModule] Connect To Server. IP=" + ClientConnection.Addess + ", Port=" + ClientConnection.Port);
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = ClientConnection.Addess;
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = ClientConnection.Port;
        NetworkingManager.Singleton.NetworkConfig.ConnectionApproval = true;
        NetworkingManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(this.ClientConnection.PlayerSessionId);
        LogModule.WriteToLogFile("[ClientModule] ConnectionData=" + NetworkingManager.Singleton.NetworkConfig.ConnectionData);
        NetworkingManager.Singleton.StartClient();
    }

    public void DisconnectToServer()
    {
        if (NetworkingManager.Singleton.IsConnectedClient)
        {
            LogModule.WriteToLogFile("[ClientModule] Disconnect CLient");
            NetworkingManager.Singleton.StopClient();
        }
    }

    private void MatchRequestCallback(string matchmakingResponse)
    {
        LogModule.WriteToLogFile("[ClientModule] Match Request Callback - " + matchmakingResponse);
        APIModule.MatchmakingResponse matchmakingResult = JsonUtility.FromJson<APIModule.MatchmakingResponse>(matchmakingResponse);
        this.ClientConnection.TicketId = matchmakingResult.ticketId;

        StartCoroutine(MatchStatusPolling());
    }

    IEnumerator MatchStatusPolling()
    {
        LogModule.WriteToLogFile("[ClientModule] Send Polling with TicketId - " + this.ClientConnection.TicketId);
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(HttpModule.PutRequest(ApiModule.GetMatchStatusAPI(),
            new APIModule.MatchstatusRequest(this.PlayerId, this.ClientConnection.TicketId), MatchStatusCallback));
    }

    private void MatchStatusCallback(string matchstatusResponse)
    {
        LogModule.WriteToLogFile("[ClientModule] MatchStatus Callback : " + matchstatusResponse);
        APIModule.MatchstatusResponse matchstatusResult = JsonUtility.FromJson<APIModule.MatchstatusResponse>(matchstatusResponse);

        if (!LocalTest)
        {
            if (matchstatusResult.port > 0)
            {
                this.ClientConnection.MatchSuccess = true;
                this.ClientConnection.Addess = matchstatusResult.address;
                this.ClientConnection.Port = matchstatusResult.port;
                this.ClientConnection.PlayerSessionId = matchstatusResult.playerSessionId;
            }
            if (this.ClientConnection.MatchSuccess)
            {
                SetMainUI(UIType.PLAY);
                DisconnectToServer();
                SceneManager.LoadScene(this.ClientConnection.WorldId);
            }
            else
            {
                if (this.ClientConnection.PollingCount < 120)
                {
                    // Send requests about 2minutes
                    StartCoroutine(MatchStatusPolling());
                }
                else
                {
                    LogModule.WriteToLogFile("[ClientModule] Connection Error. Start later");
                    if (ConnectionStatus != null)
                    {
                        ConnectionStatus.text = "Connection Error";
                    }
                }
            }
        }
    }
    #endregion
}