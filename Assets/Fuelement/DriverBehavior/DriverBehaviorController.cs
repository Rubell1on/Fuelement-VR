using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DriverBehaviorController: MonoBehaviour
{
    const string folderPath = "Fuelement/DriverBehavior/Sessions";

    public string FolderPath
    {
        get
        {
            return $"{Application.dataPath}/{folderPath}";
        }
    }
    public string Path
    {
        get 
        {
            string dateTime = DateTime.Now.ToString("yy-MM-dd HH-mm-ss");
            return $"{FolderPath}/{dateTime}.json"; 
        }
    }

    public List<DriverBehavior> driverBehaviors;
    public bool active = false;
    Dictionary<string, List<DriverActivity>> driverActivities;

    void Start()
    {
        InitDictionary();
    }

    private void Update()
    {
        if (active)
        {
            active = false;
            SaveFile();
        }
    }

    void InitDictionary()
    {
        driverActivities = new Dictionary<string, List<DriverActivity>>();
        driverBehaviors.ForEach(d =>
        {
            string className = d.GetType().Name;
            driverActivities.Add(className, new List<DriverActivity>());

            d.ErrorOccurred.AddListener(args =>
            {
                DriverActivity activity = new DriverActivity(args.currentValue, args.diffValue, new Vector3(0, 0, 0), Quaternion.identity);
                driverActivities[className].Add(activity);
            });
        });
    }

    void SaveFile()
    {
        if (!Directory.Exists(FolderPath))
        {
            DirectoryInfo info = Directory.CreateDirectory(FolderPath);
            if (!info.Exists)
            {
                Debug.LogError("Folder haven't created!");
                return;
            }
        }

        string path = Path;
        FileStream fs;

        if (!File.Exists(path))
        {
            fs = File.Create(path);
        } else
        {
            fs = File.Open(path, FileMode.OpenOrCreate);
        }

        string json = JsonConvert.SerializeObject(driverActivities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public class DriverActivity
    {
        public string time;
        public float currentValue;
        public float diffValue;
        public Vector3 carPosition;
        public Quaternion rotation;

        public DriverActivity(float currentValue, float diffValue, Vector3 carPosition, Quaternion rotation)
        {
            this.time = DateTime.Now.ToString("hh:mm:ss");
            this.currentValue = currentValue;
            this.diffValue = diffValue;
            this.carPosition = carPosition;
            this.rotation = rotation;
        }
    }
}