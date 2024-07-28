using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor.Rendering;
using Mono.Data.Sqlite;
using System;

public class CustomNetworkManager : NetworkManager
{
    public List<MovimientoPlayer> players = new List<MovimientoPlayer>();
    private string connectionString;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Configura la cadena de conexión
        connectionString = "URI=file:/home/dilan/Database/ChemicalTasksDB.db";
        VerifyDatabaseConnection();
        Debug.Log("El servidor se ha iniciado y se ha conectado a la base de datos.");
    }

    private void VerifyDatabaseConnection()
    {
        using (var connection = new Mono.Data.Sqlite.SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conexión exitosa a la base de datos.");
            }
            catch (Exception e)
            {
                Debug.LogError("Error al conectar a la base de datos: " + e.Message);
            }
        }
    }

    
}
