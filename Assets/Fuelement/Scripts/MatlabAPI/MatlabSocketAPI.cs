using System;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MatlabSocketAPI : MonoBehaviour
{
    public string host = "localhost";
    public int port = 3000;
    public SettingsMenu sm;
    public bool connected = false;
    public int reconnectTime = 5;
    public MatlabResponseEvent matlabResponseReceived;

    TcpClient socket = new TcpClient();
    NetworkStream stream;

    bool isWriting = true;

    float kMhRatio = 3.6f;
    float oldRpm = 0;
    float deltaRpm = 0;

    Coroutine reconnectCoroutine;
    Drivetrain drivetrain;

    private void Start()
    {
        //Connect();        
    }

    public void Connect()
    {
        if (sm != null)
        {
            if (sm.selectedCar != null)
            {               
                if (sm.selectedCar.TryGetComponent(out drivetrain))
                {
                    StartCoroutine(_Connect());
                } else
                {
                    Debug.LogError("Can't find car script called Drivetrain!");
                }
            } else
            {
                Debug.LogError("Cars list is empty!");
            }
        } else
        {
            Debug.LogError("StartGame script not found!");
        }
        
    }

    public void Connect(MatlabProcess.MatlabProcessState state)
    {
        if (state == MatlabProcess.MatlabProcessState.ListeningConnections)
        {
            if (sm != null)
            {
                if (sm.selectedCar != null)
                {
                    if (sm.selectedCar.TryGetComponent(out drivetrain))
                    {
                        StartCoroutine(_Connect());
                    }
                    else
                    {
                        Debug.LogError("Can't find car script called Drivetrain!");
                    }
                }
                else
                {
                    Debug.LogError("Cars list is empty!");
                }
            }
            else
            {
                Debug.LogError("StartGame script not found!");
            }
        }
    }

    IEnumerator _Connect()
    {
        try
        {
            socket = new TcpClient();
            socket.Connect(host, port);
            Debug.Log("Connected!");
            stream = socket.GetStream();
        }
        catch
        {
            TryReconnect(reconnectTime);
        }

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

                        MatlabResponse response = JsonUtility.FromJson<MatlabResponse>(message);

                        matlabResponseReceived.Invoke(response);
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

    void TryReconnect(int reconnectTime)
    {
        int rt = reconnectTime;
        reconnectCoroutine = StartCoroutine(Timers.SetInterval(() =>
        {
            Debug.Log($"Попытка переподключения к серверу через: {rt}");

            if (rt == 1)
            {
                StopCoroutine(reconnectCoroutine);
                Connect();
            }

            rt -= 1;
        }, 1));
    }
}

public static class Timers
{

    public static IEnumerator SetInterval(Action callback, float duration = 0)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            callback();
        }
    }

    public static IEnumerator SetTimeout(Action callback, float duration = 0)
    {
        yield return new WaitForSeconds(duration);
        callback();
    }
}

[Serializable]
public class MatlabResponseEvent : UnityEvent<MatlabResponse> { }