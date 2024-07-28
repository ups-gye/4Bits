using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mono.Data.Sqlite;
using UnityEngine;

public class ServerDatabaseManager : NetworkBehaviour
{

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
        using (var connection = new SqliteConnection(connectionString))
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

    [Command]
    public void CmdRequestQuestion()
    {
        if (isServer)
        {
            LoadAndSendQuestion();
        }
    }

    private void LoadAndSendQuestion()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Questions ORDER BY RANDOM() LIMIT 1";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string questionText = reader["QuestionText"].ToString();
                    string answer1 = reader["Answer1"].ToString();
                    string answer2 = reader["Answer2"].ToString();
                    string answer3 = reader["Answer3"].ToString();
                    string answer4 = reader["Answer4"].ToString();
                    
                    // Enviar la pregunta a todos los clientes conectados
                    foreach (var conn in NetworkServer.connections.Values)
                    {
                        TargetSendQuestion(conn, questionText, answer1, answer2, answer3, answer4);
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró ninguna pregunta en la base de datos.");
                }
            }
        }
    }

    [TargetRpc]
    void TargetSendQuestion(NetworkConnection conn, string questionText, string answer1, string answer2, string answer3, string answer4)
    {
        // Aquí puedes actualizar la UI del cliente con la pregunta y las respuestas recibidas
        Debug.Log($"Pregunta enviada al cliente {conn.connectionId}:");
        Debug.Log("Question: " + questionText);
        Debug.Log("Answer 1: " + answer1);
        Debug.Log("Answer 2: " + answer2);
        Debug.Log("Answer 3: " + answer3);
        Debug.Log("Answer 4: " + answer4);
    }

}
