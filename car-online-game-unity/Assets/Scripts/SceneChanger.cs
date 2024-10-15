using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void ChangeScene(string scene, NetworkConnectionToClient conn = null)
    {
        conn.Send(new SceneMessage { sceneName = this.gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });
        
        // Ładujemy nową scenę addytywnie
        conn.Send(new SceneMessage { sceneName = scene, sceneOperation = SceneOperation.LoadAdditive });
    }
}
