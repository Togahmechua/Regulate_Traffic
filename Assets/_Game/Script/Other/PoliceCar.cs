using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoliceCar : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Animator anim;
    [SerializeField] private CheckCollider checkCollider;

    #region Drag Control
    public void OnPointerDown(PointerEventData eventData)
    {
        Honk();
    }
    #endregion


    #region Anim
    public void TurnRight()
    {
        anim.Play(CacheString.TAG_TURNRIGHT);
    }

    public void TurnLeft()
    {
        anim.Play(CacheString.TAG_TURNLEFT);
    }

    private void Honk()
    {
        anim.Play(CacheString.TAG_HONK);
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        Car car = Cache.GetCar(other);
        if (car != null)
        {
            car.Die();
        }
    }
}
