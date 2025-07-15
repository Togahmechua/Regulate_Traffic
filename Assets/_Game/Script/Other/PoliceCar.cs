using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoliceCar : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Animator anim;
    [SerializeField] private CheckCollider checkCollider;

    private int MaxHP = 3;

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
        PlayDriftSFX();
    }

    public void TurnLeft()
    {
        anim.Play(CacheString.TAG_TURNLEFT);
        PlayDriftSFX();
    }

    private void Honk()
    {
        anim.Play(CacheString.TAG_HONK);
    }

    public void PlayHornMusic()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.horn);
    }

    public void PlayDriftSFX()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.drift);
    }
    #endregion

    private void TakeDamge()
    {
        MaxHP--;
        if (MaxHP <= 0)
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.dead);
            UIManager.Ins.mainCanvas.Hit();
            Debug.Log("Die");

            UIManager.Ins.TransitionUI<ChangeUICanvas, MainCanvas>(0.5f,
                () =>
                {
                    LevelManager.Ins.DespawmLevel();
                    UIManager.Ins.OpenUI<LooseCanvas>();
                });
        }
        else
        {
            UIManager.Ins.mainCanvas.Hit();
            Debug.Log("Take Damge");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Car car = Cache.GetCar(other);
        if (car != null)
        {
            TakeDamge();
            car.Die();
            AudioManager.Ins.PlaySFX(AudioManager.Ins.hurt);
        }
    }
}
