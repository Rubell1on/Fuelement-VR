using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : Singleton<LevelSettings>
{
    public GameObject spawnPoint;
    public CustomCamera customCamera;
    public GameObject carInstance;
    [SerializeField]
    private CarData carData;
    [SerializeField]
    private TasksController tasksController;

    public new void Awake()
    {
        SetDoNotDestroyOnLoad(false);
        base.Awake();
    }

    public void SetCar(CarData car)
    {
        this.carData = car;
        carInstance = Instantiate(car.car, Vector3.zero, Quaternion.identity, spawnPoint.transform);

        if (carInstance != null)
        {
            customCamera.target = carInstance.transform;
            CamerasController camerasController = carInstance.GetComponent<CamerasController>();
            camerasController.customCamera = customCamera;
        }
    }

    public void SetTitle(string title)
    {
        tasksController.Title = title;
    }
}
