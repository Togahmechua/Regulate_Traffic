using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : UICanvas
{
    [SerializeField] private Button startBtn;
    private Animator anim;


    private void OnEnable()
    {
        startBtn.interactable = true;    
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        startBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            startBtn.interactable = false;
            anim.Play(CacheString.TAG_PLAY);
        });
    }

    public void Play()
    {
        UIManager.Ins.TransitionUI<ChangeUICanvas, StartCanvas>(0.5f,
                () =>
                {
                    UIManager.Ins.OpenUI<MainCanvas>();
                    LevelManager.Ins.SpawnLevel();
                });
    }
}
