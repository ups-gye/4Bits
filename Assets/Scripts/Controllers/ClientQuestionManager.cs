using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ClientQuestionManager : NetworkBehaviour
{

    public Text questionText;
    public Text answer1Text;
    public Text answer2Text;
    public Text answer3Text;
    public Text answer4Text;

    void Start()
    {
        if (isLocalPlayer)
        {
            RequestQuestion();
        }
    }

    [Client]
    void RequestQuestion()
    {
        if (isLocalPlayer)
        {
            CmdRequestQuestion();
        }
    }

    [Command]
    void CmdRequestQuestion()
    {
        var serverDatabaseManager = NetworkManager.singleton.GetComponent<ServerDatabaseManager>();
        serverDatabaseManager.CmdRequestQuestion();
    }

    [TargetRpc]
    public void TargetReceiveQuestion(NetworkConnection target, string question, string answer1, string answer2, string answer3, string answer4)
    {
        // Actualizar la UI con la pregunta y las respuestas
        questionText.text = question;
        answer1Text.text = answer1;
        answer2Text.text = answer2;
        answer3Text.text = answer3;
        answer4Text.text = answer4;
    }
    
}
