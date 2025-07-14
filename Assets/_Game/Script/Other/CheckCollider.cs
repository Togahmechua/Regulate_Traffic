using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private Car curCar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Car car = Cache.GetCar(collision);
        if (car != null && curCar != car)
        {
            curCar = car;
            car.ChangeLane();
        }
    }
}
