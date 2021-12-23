using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class SQLConnection
{
    private static string connectionString;
    public static ConnectionData connectionData = new ConnectionData();

    private static readonly string key = "keyConnectionString";
    private static readonly string filePath = "connectionString";

    public static async Task<MySqlConnection> GetConnection()
    {
        connectionString = GetConnectionString();
        ConnectionData possibleConnectionData = ConnectionData.Parse(connectionString);

        bool equal = !connectionData.Equals(possibleConnectionData);
        if (equal) 
        {
            connectionData = possibleConnectionData;
        }

        MySqlConnection connection = null;

        try
        {
            return await Task.Run<MySqlConnection>(() =>
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                return connection;
            });
        }
        catch (Exception e)
        {
            Debug.LogError("Состояние подключения: " + connection.State);
            Debug.LogError("Ошиибка подключения: " + e);

            return null;
        }
    }

    public static string GetConnectionString()
    {
        string connectionString = UnityEngine.Resources.Load(filePath).ToString();

        if (connectionString.Length == 0)
        {
            connectionString = PlayerPrefs.GetString(key);
        }

        return connectionString;
    }

    public static void SetConnectionString(string connectionString)
    {
        PlayerPrefs.SetString(key, connectionString);
    }

    public static bool ConnectionStringExists()
    {
        return UnityEngine.Resources.Load(filePath) != null || PlayerPrefs.HasKey(key) ? true : false;
    }
}