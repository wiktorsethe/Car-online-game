using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : NetworkBehaviour
{
    // Zmienna do przechowywania nazwy poprzedniej sceny
    private string previousScene;

    [Command(requiresAuthority = false)]
    public void ChangeScene(string scene, NetworkConnectionToClient conn = null)
    {
        /*// Jeśli mamy poprzednią scenę, wyładowujemy ją
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.UnloadSceneAsync(previousScene);
        }*/

        conn.Send(new SceneMessage { sceneName = this.gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });
        //NetworkServer.RemovePlayerForConnection(conn, false);
        
        // Ładujemy nową scenę addytywnie
        conn.Send(new SceneMessage { sceneName = scene, sceneOperation = SceneOperation.LoadAdditive });
        //NetworkServer.AddPlayerForConnection(conn, player);
        

        // Przechowujemy nazwę nowej sceny jako poprzednią scenę na przyszłość
        previousScene = scene;
    }
}
