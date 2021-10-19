using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DriverBehaviorController: MonoBehaviour
{
    public int userId = 0;
    const string folderPath = "Fuelement/DriverBehavior/Sessions";

    public string FolderPath
    {
        get
        {
            string currentDate = DateTime.Now.ToString("yy-MM-dd");
            return $"{Application.dataPath}/{folderPath}/{userId}/{currentDate}";
        }
    }
    public string Path
    {
        get 
        {
            string dateTime = DateTime.Now.ToString("HH-mm-ss");
            return $"{FolderPath}/{dateTime}.json"; 
        }
    }

    public List<DriverBehavior> driverBehaviors;
    public bool active = false;
    Dictionary<string, DriverActivity> driverActivities;

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
        driverActivities = new Dictionary<string, DriverActivity>();
        driverBehaviors.ForEach(d =>
        {
            string className = d.GetType().Name;
            driverActivities.Add(className, new DriverActivity());

            d.ErrorOccurred.AddListener(args =>
            {
                DriverActivity activity = driverActivities[className];

                activity.missteps.Add(new DriverMisstep(args.currentValue, args.diffValue, new Vector3(0, 0, 0), Quaternion.identity));

                DriverBehavior driverBehavior = (DriverBehavior)args.sender;
                activity.errorsCount = driverBehavior.errorsCount;
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

    public class User
    {
        public int id;

    }

    public class DriverActivity
    {
        public int errorsCount;
        public List<DriverMisstep> missteps = new List<DriverMisstep>();

        public DriverActivity() { }
    }

    public class DriverMisstep
    {
        public string time;
        public float currentValue;
        public float diffValue;
        public Vector3 carPosition;
        public Quaternion rotation;

        public DriverMisstep(float currentValue, float diffValue, Vector3 carPosition, Quaternion rotation)
        {
            this.time = DateTime.Now.ToString("hh:mm:ss");
            this.currentValue = currentValue;
            this.diffValue = diffValue;
            this.carPosition = carPosition;
            this.rotation = rotation;
        }
    }
}