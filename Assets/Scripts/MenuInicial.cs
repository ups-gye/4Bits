using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void CreateRoom()
    {
        SceneManager.LoadScene("CreateRoom");
    }

    public void JoinRoom() 
    {
        SceneManager.LoadScene("JoinRoom");
    }

    public void ExitGame() 
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    public void BackToMainMenu() 
    {
        Debug.Log("Se regresó al menú inicial");
        SceneManager.LoadScene("MainMenu");
    }
}
