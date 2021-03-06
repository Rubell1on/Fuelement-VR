using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class SocketClient
{
    private string host;
    private int port;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public SocketClient(string host, int port)
    {
        this.host = host;
        this.port = port;
    }

    public void Connect()
    {
        socket = new TcpClient(this.host, this.port);
        while (!socket.Connected)
        {
            Debug.Log($"Waiting for connection with {this.host}:{this.port}");
        }

        this.stream = socket.GetStream();
        this.writer = new StreamWriter(stream);
        this.reader = new StreamReader(stream);
    }

    public void WriteLine(string msg)
    {
        writer.WriteLine(msg);
    }
    public void WriteLine(float value)
    {
        writer.WriteLine(value);
    }

    public void Read()
    {
        this.reader.Read();
    }

    public void Flush()
    {
        writer.Flush();
    }

    public void Close()
    {
        this.writer.Close();
    }
}
