using System.Collections.Generic;
using UnityEngine;

public class CarScriptsController : MonoBehaviour
{
    public Drivetrain drivetrain;
    public AerodynamicResistance aerodynamicResistance;
    public SoundController soundController;
    public CarDynamics carDynamics;
    public Axles axles;
    public AxisCarController axisCarController;
    public CarDamage carDamage;
    public BrakeLights brakeLights;
    public Arcader arcader;
    public Setup setup;
    public Rigidbody bodyRb;
    public List<Wheel> wheels;
    public DashBoard dashBoard;
    public FuelTank fuelTank;
    public List<Wing> wings;
    public CamerasController camerasController;

    void Start()
    {
        if (brakeLights == null) brakeLights = GetComponent<BrakeLights>();
        if (drivetrain == null) drivetrain = GetComponent<Drivetrain>();
        if (aerodynamicResistance == null) aerodynamicResistance = GetComponent<AerodynamicResistance>();
        if (soundController == null) soundController = GetComponent<SoundController>();
        if (carDynamics == null) carDynamics = GetComponent<CarDynamics>();
        if (axles == null) axles = GetComponent<Axles>();
        if (axisCarController == null) axisCarController = GetComponent<AxisCarController>();
        if (carDamage == null) carDamage = GetComponent<CarDamage>();
        if (arcader == null) arcader = GetComponent<Arcader>();
        if (setup == null) setup = GetComponent<Setup>();
        if (bodyRb == null) bodyRb = GetComponent<Rigidbody>();
        if (camerasController == null) camerasController = GetComponent<CamerasController>();
    }

    public void EnableCardMode()
    {
        if (brakeLights != null) brakeLights.enabled = false;
        if (drivetrain != null) drivetrain.enabled = false;
        if (aerodynamicResistance != null) aerodynamicResistance.enabled = false;
        if(soundController != null) soundController.enabled = false;
        if (carDynamics != null) carDynamics.enabled = false;
        if (axles != null) axles.enabled = false;
        if (axisCarController != null) axisCarController.enabled = false;
        if (carDamage != null) carDamage.enabled = false;
        if (arcader != null) arcader.enabled = false;
        if (setup != null) setup.enabled = false;
        if (bodyRb != null) bodyRb.isKinematic = true;
        if (wheels.Count != 0) wheels.ForEach(w => w.enabled = false);
        if (dashBoard != null) dashBoard.enabled = false;
        if (wings.Count != 0) wings.ForEach(w => w.enabled = false);
    }
}
