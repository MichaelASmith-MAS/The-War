using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed
{
    public int maxSpeed, currentSpeed;

    public Speed()
    {
        maxSpeed = 100;
        currentSpeed = maxSpeed;
    }

    public Speed(int maxSpeed)
    {
        this.maxSpeed = maxSpeed;
        currentSpeed = this.maxSpeed;
    }

}