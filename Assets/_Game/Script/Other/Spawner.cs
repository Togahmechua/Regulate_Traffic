using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;

    [Header("Cài đặt thời gian spawn")]
    [SerializeField] private float initialDelay = 3.0f;     // Ban đầu spawn chậm
    [SerializeField] private float minDelay = 1.5f;         // Không nhanh hơn 1.5s
    [SerializeField] private float decreaseRate = 0.1f;     // Mỗi lần giảm delay
    [SerializeField] private float decreaseInterval = 5f;   // Mỗi 5s thì giảm 1 lần

    private float currentDelay;
    private float decreaseTimer;

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
                //Debug.Log("⬇️ Tăng tốc độ spawn! Delay mới: " + currentDelay.ToString("F2") + "s");
            }
        }
    }

    private void SpawnCar()
    {
        int randIndex = Random.Range(0, spawnPos.Length);
        Vector3 pos = spawnPos[randIndex].position;

        Car newCar = SimplePool.Spawn<Car>(PoolType.Car, pos, Quaternion.identity);
        newCar.SetCurPos(pos, randIndex);
    }
}
