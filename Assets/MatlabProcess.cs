using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatlabProcess : MonoBehaviour
{
    public string workingDirectory = "H://Unity Projects/Fuelement-VR/Assets/Fuelement/Scripts/MatlabAPI/matlab";
    public enum MatlabProcessState {Stopped, Starting, LoadingFIS, LaunchingServer, ListeningConnections, Connected };

    public MatlabProcessState state = MatlabProcessState.Stopped;
    public OnStateChangeEvent onStateChanged;

    Process matlab;
    Dictionary<string, MatlabProcessState> strState = new Dictionary<string, MatlabProcessState>()
    {
        { "Starting up", MatlabProcessState.Starting },
        { "Loading FIS", MatlabProcessState.LoadingFIS },
        { "Openning connection", MatlabProcessState.LaunchingServer },
        { "Waiting for connection", MatlabProcessState.ListeningConnections },
        { "Connection established", MatlabProcessState.Connected }
    };

    MatlabProcessState prevState = MatlabProcessState.Stopped;

    void Start()
    {
        Run();
    }

    void Run()
    {
        matlab = new Process();
        matlab.EnableRaisingEvents = false;
        ProcessStartInfo startInfo = new ProcessStartInfo("matlab", "-batch server");
        startInfo.CreateNoWindow = true;
        startInfo.WorkingDirectory = workingDirectory;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        matlab.StartInfo = startInfo;

        matlab.OutputDataReceived += OnOutputDataReceived;
        matlab.ErrorDataReceived += OnErrorDataReceived;
        matlab.Exited += OnExit;

        matlab.Start();
        matlab.BeginOutputReadLine();
        matlab.BeginErrorReadLine();
    }

    void OnOutputDataReceived(object o, DataReceivedEventArgs e)
    {
        string s = e.Data;
        if (!String.IsNullOrEmpty(s))
        {
            if (strState.ContainsKey(s))
            {
                state = strState[s];
                UnityEngine.Debug.Log(s);
            }
        }
    }

    void OnErrorDataReceived(object o, DataReceivedEventArgs e)
    {
        string s = e.Data;
        UnityEngine.Debug.LogError(s);
    }

    void OnExit(object o, EventArgs e)
    {
        UnityEngine.Debug.LogError(e);
    }

    void Update()
    {
        if (prevState != state)
        {
            onStateChanged.Invoke(state);
            prevState = state;
        }
    }

    [Serializable]
    public class OnStateChangeEvent : UnityEvent<MatlabProcessState> { }
}
