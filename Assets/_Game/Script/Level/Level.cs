using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] carSpawnPos;
    [SerializeField] private Canvas cv;
    [SerializeField] Spawner spawner;

    private void OnEnable()
    {
        cv.renderMode = RenderMode.ScreenSpaceCamera;
        cv.worldCamera = Camera.main;
    }

    private void Start()
    {
        UIManager.Ins.mainCanvas.ResetUI();
    }

    public float GetLaneX(int index)
    {
        index = Mathf.Clamp(index, 0, carSpawnPos.Length - 1);
        return carSpawnPos[index].position.x;
    }

    public Vector3 GetSpawnPos(int index)
    {
        return carSpawnPos[index].position;
    }

    public void DespawnAll()
    {
        spawner.DespawnAllSpawnedCars();
    }
}
