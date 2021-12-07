using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : Singleton<LevelSettings>
{
    private CarData carData;
    public GameObject spawnPoint;
    public CustomCamera customCamera;
    public GameObject carInstance;

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
}
