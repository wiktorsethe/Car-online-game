using System.Collections;
using System.IO;
using Mirror;
using Mirror.Authenticators;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{
    private string serverUrl = "http://54.38.52.204/getlogin.php";
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject canvas;
    [SerializeField] private InputField loginField;
    [SerializeField] private InputField passwordField;
    //[SerializeField] private GameObject selectCharacterMenu;
    
    public AdditiveNetworkManager myNetworkManagerScript;
    /*private FadeInOutScreen fadeInOutScreenScript;
    [Scene] public string transitionToSceneName;
    public string scenePosToSpawnOn;

    public GameObject player;*/
    
    private void Awake()
    {
        if (myNetworkManagerScript == null)
        {
            myNetworkManagerScript = FindObjectOfType<AdditiveNetworkManager>();
            //fadeInOutScreenScript = FindObjectOfType<FadeInOutScreen>();
        }
    }

    public void Start()
    {
        Button btn = loginButton.GetComponent<Button>();
        btn.onClick.AddListener(LoginOnClick);
    }

    private void LoginOnClick()
    {
        StartCoroutine(LoginDB(loginField.text, passwordField.text));
    }
    IEnumerator LoginDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        WWW www = new WWW(serverUrl, form);

        yield return www;

        string[] userData = www.text.Split('|');
        if (userData.Length == 3)
        {
            

            
            var auth = FindObjectOfType<CustomAuthenticator>();
            auth.SetPlayerId(userData[0]);
            //auth.SetPassword(userData[2]);

            myNetworkManagerScript.StartClient();

            /*var ni = NetworkClient.connection.identity;
            //player = ni.gameObject;
            ni.GetComponent<UserInfo>().CmdSetUserId(userData[0]);
            ni.GetComponent<UserInfo>().CmdSetLogin(userData[1]);
            ni.GetComponent<UserInfo>().CmdSetPassword(userData[2]);*/
            
            /*if (isServer)
            {
                StartCoroutine(SendPlayerToNewScene(player));
            }
            else
            {
                Debug.Log("Player has been logged in, waiting for server.");
            }*/
        }
        else
        {
            loginField.text = "";
            passwordField.text = "";
            Debug.Log(www.text);
        }
    }
    
    /*
    [ServerCallback]
    IEnumerator SendPlayerToNewScene(GameObject player)
    {
        if (player.TryGetComponent(out NetworkIdentity identity))
        {
            NetworkConnectionToClient conn = identity.connectionToClient;
            
            
            Debug.Log("Start moving the player to a new scene: " + transitionToSceneName);

            conn.Send(new SceneMessage { sceneName = gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });


            yield return new WaitForSeconds((fadeInOutScreenScript.speed * 0.1f));

            NetworkServer.RemovePlayerForConnection(conn, false);



            NetworkStartPosition[] allStartPos = FindObjectsOfType<NetworkStartPosition>();

            Transform start = myNetworkManagerScript.GetStartPosition();
            foreach (var item in allStartPos)
            {
                if (item.gameObject.scene.name == Path.GetFileNameWithoutExtension(transitionToSceneName) && item.name == scenePosToSpawnOn)
                {
                    start = item.transform;
                }
            }

            player.transform.position = start.position;


            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByPath(transitionToSceneName));

            Debug.Log("Player moved to new scene: " + transitionToSceneName);
            
            
            conn.Send(new SceneMessage { sceneName = transitionToSceneName, sceneOperation = SceneOperation.LoadAdditive, customHandling = true });


            NetworkServer.AddPlayerForConnection(conn, player);

            Debug.Log("Player added back to server after scene change.");
        }
        else
        {
            Debug.LogError("NetworkIdentity component not found for player: " + player.name);
        }
    }*/

}