using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Car car = Cache.GetCar(collision);
        if (car != null && car.isAbleToTurn)
        {
            car.TryForceChangeLane();
        }
    }
}
