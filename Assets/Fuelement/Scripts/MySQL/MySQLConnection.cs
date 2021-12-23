using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MySQLConnection : MonoBehaviour
{
    public InputField ip;
    public InputField dbName;
    public InputField user;
    public InputField password;
    public Text textState;

    private ConnectionData connectionData;

    private void Start()
    {
        CheckRegistry();
        connectionData = ConnectionData.Parse(SQLConnection.GetConnectionString());
        CheckConnection();
        //FillFieldUI();
    }

    public void CheckRegistry()
    {
        if (!SQLConnection.ConnectionStringExists())
        {
            SetStringConnection(new ConnectionData().ToString());
        }
    }

    private void FillFieldUI()
    {
        ip.text = connectionData.server;
        dbName.text = connectionData.database;
        user.text = connectionData.user;
        password.text = connectionData.password;
    }

    public void SaveParametrs()
    {
        connectionData.server = ip.text;
        connectionData.database = dbName.text;
        connectionData.user = user.text;
        connectionData.password = password.text;
        SetStringConnection(connectionData.ToString());
        Debug.Log("��������� �����������, ������� ���������.");
    }

    private void SetStringConnection(string connection)
    {
        SQLConnection.SetConnectionString(connection);
    }

    public async void CheckConnection()
    {
        if (await SQLConnection.GetConnection() == null)
        {
            Debug.LogError("���������� � MySQL �������");
            //textState.color = Color.red;
            //textState.text = "�������";
        }
        else
        {
            Debug.Log("���������� � MySQL �������");
            //textState.color = Color.green;
            //textState.text = "�������";
        }

    }
}
