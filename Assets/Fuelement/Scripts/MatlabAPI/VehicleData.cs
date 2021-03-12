using System;

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
