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
    [HideInInspector]
    public Dictionary<string, DriverActivity> driverActivities;

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
            DriverActivity activity = new DriverActivity();
            activity.behaviorName = d.behaviorName;

            driverActivities.Add(className, activity);

            d.maxValueChanged.AddListener(args =>
            {
                DriverBehavior driverBehavior = (DriverBehavior)args.sender;
                activity.maxValue = driverBehavior.maxValue;
            });

            d.minValueChanged.AddListener(args =>
            {
                DriverBehavior driverBehavior = (DriverBehavior)args.sender;
                activity.minValue = driverBehavior.minValue;
            });

            d.avgValueChanged.AddListener(args =>
            {
                DriverBehavior driverBehavior = (DriverBehavior)args.sender;
                activity.avgValue = driverBehavior.avg;
            });

            d.ErrorOccurred.AddListener(args =>
            {
                DriverBehavior driverBehavior = (DriverBehavior)args.sender;

                activity.missteps.Add(new DriverMisstep(driverBehavior.CurrentValue, driverBehavior.diffValue, new Vector3(0, 0, 0), Quaternion.identity));

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
        public string behaviorName;
        public float minValue;
        public float maxValue;
        public float avgValue;
        public int errorsCount;
        public List<DriverMisstep> missteps = new List<DriverMisstep>();
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