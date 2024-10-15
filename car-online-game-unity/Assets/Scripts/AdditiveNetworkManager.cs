using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class AdditiveNetworkManager : NetworkManager
{
     public string StartScene;
    
    private string[] scenesToLoad;
    private bool subscenesLoaded;
    /*private readonly List<Scene> subScenes = new List<Scene>();*/

    private bool isInTransition;
    private bool firstSceneLoaded;

    private string serverUrlLogout = "http://54.38.52.204/getlogout.php";
    private string playerID;
    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) 
        {
            // Automatyczny start serwera w trybie headless
            StartServer();
        }
        
        int sceneCount = SceneManager.sceneCountInBuildSettings - 2; //Subtract the offline and persistent scene
        scenesToLoad = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenesToLoad[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i + 2));
        }
    }
    
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        if (sceneName == onlineScene)
        {
            StartCoroutine(ServerLoadSubScene());
        }
    }

    public override void OnClientSceneChanged()
    {
        if (isInTransition == false)
        {
            base.OnClientSceneChanged();
        }
    }

    IEnumerator ServerLoadSubScene()
    {
        foreach (var additiveScene in scenesToLoad)
        {
            yield return SceneManager.LoadSceneAsync(additiveScene, new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive,
                localPhysicsMode = LocalPhysicsMode.Physics2D
            });
        }

        subscenesLoaded = true;
    }

    public override void OnClientChangeScene(string sceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if (sceneOperation == SceneOperation.UnloadAdditive)
            StartCoroutine(UnloadAdditive(sceneName));
        
        if (sceneOperation == SceneOperation.LoadAdditive)
            StartCoroutine(LoadAdditive(sceneName));
    }
    
    IEnumerator LoadAdditive(string sceneName)
    {
        isInTransition = true;

        if (mode == NetworkManagerMode.ClientOnly)
        {
            loadingSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
            {
                yield return null;
            }
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false;
        
        OnClientSceneChanged();

        if (firstSceneLoaded == false)
        {
            firstSceneLoaded = true;
            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            firstSceneLoaded = true;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator UnloadAdditive(string sceneName)
    {
        isInTransition = true;

        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false;
        
        OnClientSceneChanged();
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        if (conn.identity == null)
            StartCoroutine(AddPlayerDelayed(conn));
    }

    IEnumerator AddPlayerDelayed(NetworkConnectionToClient conn)
    {
        while (subscenesLoaded == false)
            yield return null;

        NetworkIdentity[] allObjWithNetworkIdentity = FindObjectsOfType<NetworkIdentity>();

        foreach (var item in allObjWithNetworkIdentity)
        {
            item.enabled = true;
        }

        firstSceneLoaded = false;
        
        conn.Send(new SceneMessage{ sceneName = StartScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true});

        Transform startPos = GetStartPosition();

        GameObject player = Instantiate(playerPrefab, startPos);
        player.transform.SetParent(null);

        yield return new WaitForEndOfFrame();
        
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(StartScene));

        NetworkServer.AddPlayerForConnection(conn, player);
    }
    
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        playerID = (string)conn.authenticationData;
        
        if (!string.IsNullOrEmpty(playerID) && CustomAuthenticator.playerIDs.Contains(playerID))
        {
            // Znajdź odpowiadającą nazwę gracza i usuń z HashSetów
            CustomAuthenticator.playerIDs.Remove(playerID);

            string playerName = CustomAuthenticator.playerNames.FirstOrDefault(name => conn.authenticationData.Equals(playerID));
            if (!string.IsNullOrEmpty(playerName))
            {
                CustomAuthenticator.playerNames.Remove(playerName);
            }
        }
        
        StartCoroutine(SendLogoutRequest());
    }
    private IEnumerator SendLogoutRequest()
    {
        WWWForm form = new WWWForm();

        form.AddField("userId", playerID);
        new WWW(serverUrlLogout, form);
        return null;
    }
}
