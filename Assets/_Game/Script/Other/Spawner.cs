using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;

    [Header("Cài đặt thời gian spawn")]
    [SerializeField] private float initialDelay = 3.0f;
    [SerializeField] private float minDelay = 1.5f;
    [SerializeField] private float decreaseRate = 0.1f;
    [SerializeField] private float decreaseInterval = 5f;

    private float currentDelay;
    private float decreaseTimer;

    [SerializeField] private List<Car> spawnedCars = new List<Car>();

    private void Start()
    {
        currentDelay = initialDelay;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnCar();
            yield return new WaitForSeconds(currentDelay);

            decreaseTimer += currentDelay;

            if (decreaseTimer >= decreaseInterval)
            {
                decreaseTimer = 0f;
                currentDelay = Mathf.Max(minDelay, currentDelay - decreaseRate);
            }
        }
    }

    private void SpawnCar()
    {
        int randIndex = Random.Range(0, spawnPos.Length);
        Vector3 pos = spawnPos[randIndex].position;

        Car newCar = SimplePool.Spawn<Car>(PoolType.Car, pos, Quaternion.identity);
        newCar.SetCurPos(pos, randIndex);

        spawnedCars.Add(newCar); // ✅ Thêm vào danh sách
    }

    public void DespawnAllSpawnedCars()
    {
        foreach (Car car in spawnedCars)
        {
            if (car != null && car.gameObject.activeSelf)
            {
                SimplePool.Despawn(car);
            }
        }

        spawnedCars.Clear(); // ✅ Dọn danh sách sau khi despawn
    }
}
