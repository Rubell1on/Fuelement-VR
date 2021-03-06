using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections;

public class MatlabSocketAPI : MonoBehaviour
{
    public string host = "localhost";
    public int port = 3000;
    public Drivetrain drivetrain;
    public bool connected = false;

    TcpClient socket = new TcpClient();
    NetworkStream stream;

    bool isWriting = true;

    float kMhRatio = 3.6f;
    float oldRpm = 0;
    float deltaRpm = 0;

    public void Connect(MatlabProcess.MatlabProcessState state)
    {
        if (state == MatlabProcess.MatlabProcessState.ListeningConnections)
        {
            StartCoroutine(_Connect());
        }
    }

    IEnumerator _Connect()
    {
        socket = new TcpClient();
        socket.Connect(host, port);
        Debug.Log("Connected!");
        stream = socket.GetStream();

        while (true)
        {
            try
            {
                if (isWriting)
                {
                    deltaRpm = drivetrain.rpm - oldRpm;
                    oldRpm = drivetrain.rpm;
                    VehicleData obj = new VehicleData(drivetrain.rpm, drivetrain.gear - 1, drivetrain.velo * kMhRatio, deltaRpm);
                    string wrapped = JsonUtility.ToJson(obj);

                    byte[] bytes = Encoding.UTF8.GetBytes(wrapped);

                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    isWriting = false;
                }
                else
                {
                    if (stream.CanRead)
                    {
                        byte[] bytes = new byte[512];
                        int nuberOfBytesRead = 0;
                        string message = "";

                        while (stream.DataAvailable)
                        {
                            nuberOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                            message += Encoding.UTF8.GetString(bytes, 0, nuberOfBytesRead);
                        }

                        Debug.Log($"Answer: {message}");
                    }

                    isWriting = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

[Serializable]
public class VehicleData
{
    public float rpm = 0;
    public int gear = 0;
    public float speed = 0;
    public float deltaRpm = 0;

    public VehicleData(float rpm, int gear, float speed, float deltaRpm)
    {
        this.rpm = rpm;
        this.gear = gear;
        this.speed = speed;
        this.deltaRpm = deltaRpm;
    }
}