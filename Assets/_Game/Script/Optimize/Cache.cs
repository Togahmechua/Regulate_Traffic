using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<Collider2D, Car> coral = new Dictionary<Collider2D, Car>();

    public static Car GetCar(Collider2D collider)
    {
        if (!coral.ContainsKey(collider))
        {
            coral.Add(collider, collider.GetComponent<Car>());
        }

        return coral[collider];
    }
}
