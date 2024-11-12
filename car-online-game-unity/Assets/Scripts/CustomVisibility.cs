using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomVisibility : NetworkBehaviour
{
    // Lista obserwatorów, którzy widzą ten obiekt
    public bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.identity != null)
            {
                // Sprawdź, czy gracz jest w tej samej scenie co obiekt
                if (conn.identity.gameObject.scene == gameObject.scene)
                {
                    observers.Add(conn);
                }
            }
        }
        return true;
    }

    // Kontroluj widoczność lokalnego gracza (hosta)
    public void OnSetLocalVisibility(bool visible)
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = visible;
        }
    }
}