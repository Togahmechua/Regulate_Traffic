using UnityEngine;
using System.Collections;

public class Car : GameUnit
{
    [Header("=== Graphics ===")]
    [SerializeField] private Transform model;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Animator anim;

    [Header("=== Movement ===")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float despawnY = -15f;
    [SerializeField] private EPath currentLane;

    public bool isAbleToTurn;

    private int dir;
    private Coroutine rotateCoroutine;

    private void OnEnable()
    {
        isAbleToTurn = true;
        anim.Play(CacheString.TAG_Idle_EnemyCar);
        spr.enabled = true;
        RandSpr();
        if (model) model.rotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < despawnY)
            SimplePool.Despawn(this);
    }

    public void ChangeLane()
    {
        if (IsAbleToChangeLane(out int direction))
        {
            Turn(direction);
        }
    }

    private bool IsAbleToChangeLane(out int direction)
    {
        direction = dir;

        switch (currentLane)
        {
            case EPath.Left: direction = 1; return true;
            case EPath.Right: direction = -1; return true;
            case EPath.Middle: return true;
        }

        return false;
    }

    private void Turn(int dir)
    {
        anim.enabled = false;

        int currentIndex = (int)currentLane;
        int newIndex = Mathf.Clamp(currentIndex + dir, 0, 2);
        float newX = LevelManager.Ins.curLevel.GetLaneX(newIndex);
        currentLane = (EPath)newIndex;

        transform.position = new Vector2(newX, transform.position.y);

        Debug.Log($"🚗 Chuyển làn: {(EPath)currentIndex} ➡ {currentLane}");

        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(RotateModel(dir));
    }


    private IEnumerator RotateModel(int dir)
    {
        float angle = dir < 0 ? 10.23f : -10.23f;
        float duration = 0.1f;
        float t = 0f;

        Quaternion startRot = model.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, angle);

        // Xoay nghiêng
        while (t < duration)
        {
            t += Time.deltaTime;
            model.rotation = Quaternion.Lerp(startRot, endRot, t / duration);
            yield return null;
        }

        // Trở về thẳng
        yield return new WaitForSeconds(0.1f);

        t = 0f;
        startRot = model.rotation;
        endRot = Quaternion.identity;

        while (t < duration)
        {
            t += Time.deltaTime;
            model.rotation = Quaternion.Lerp(startRot, endRot, t / duration);
            yield return null;
        }

        model.rotation = Quaternion.identity;

        anim.enabled = true;
    }

    public void TryForceChangeLane()
    {
        // 1/3 xác suất
        if (Random.value <= 1f / 2f)
        {
            Debug.Log("📢 Xe bị ép chuyển làn do còi 🚗");
            ChangeLane();
        }
        else
        {
            Debug.Log("📢 Xe NGHE còi nhưng KHÔNG chuyển làn ❌");
        }

        isAbleToTurn = false;
    }

    public void SetCurPos(Vector3 spawnPos, int randIndex)
    {
        transform.position = spawnPos;
        currentLane = (EPath)randIndex;

        switch (currentLane)
        {
            case EPath.Left: dir = 1; break;
            case EPath.Right: dir = -1; break;
            case EPath.Middle: dir = Random.value > 0.5f ? 1 : -1; break;
        }
    }

    private void RandSpr()
    {
        if (sprites.Length > 0)
            spr.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public void Die()
    {
        anim.Play(CacheString.TAG_BROKEN); // Vẫn dùng Animator
    }
}

public enum EPath { Left, Middle, Right }
