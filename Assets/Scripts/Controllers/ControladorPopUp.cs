using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ControladorPopUp : NetworkBehaviour {      
    public GameObject popUp;
    public Button botonCompletarTarea;
    private SyncVarPlayers syncVarPlayers;

    private void Start()
    {
        if (botonCompletarTarea != null)
        {
            botonCompletarTarea.onClick.AddListener(CompletarTarea);
        }
        else
        {
            Debug.LogError("Botón para completar tarea no asignado en ControladorPopUp");
        }
    }

    public void MostrarPopUp(SyncVarPlayers playerSyncVar)
    {
        syncVarPlayers = playerSyncVar;
        popUp.SetActive(true);

        if (isLocalPlayer)
        {
            // Solicitar una pregunta al servidor
            CmdRequestQuestion();
        }
    }

    public void CompletarTarea()
    {
        if (syncVarPlayers != null)
        {
            if (NetworkClient.localPlayer != null && NetworkClient.localPlayer.GetComponent<SyncVarPlayers>() == syncVarPlayers)
            {
                syncVarPlayers.CompleteTask();
                Debug.Log("CompletarTarea method called from ControladorPopUp.");
            }
            else
            {
                Debug.LogError("SyncVarPlayers no encontrado o el jugador local no está asignado.");
            }
        }
        else
        {
            Debug.LogError("SyncVarPlayers no encontrado en ControladorPopUp");
        }

        OcultarPopUp();
    }

    public void OcultarPopUp()
    {
        popUp.SetActive(false);
    }

    [Command]
    void CmdRequestQuestion()
    {
        // Solicitar una pregunta al servidor
        var serverDBManager = NetworkManager.singleton.GetComponent<ServerDatabaseManager>();
        serverDBManager.CmdRequestQuestion();
    }

}
