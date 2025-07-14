using UnityEngine;

public class Car : GameUnit
{
    private int currentIndex;

    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float despawnY = -10f;

    [SerializeField] private Animator anim;

    private float curPos;
    private float[] laneX = new float[] { -1.682708f, 0f, 1.682708f };
    [SerializeField] private int dir;

    private void OnEnable()
    {
        RandSpr();
        anim.Play(CacheString.TAG_Idle_EnemyCar);
       
    }

    private void Update()
    {
        // Di chuyển xuống trục Y
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Nếu vượt quá màn hình thì despawn
        if (transform.position.y < despawnY)
        {
            SimplePool.Despawn(this);
        }
    }

    public void ChangeLane()
    {
        IsAbleToChangeLane(dir);
    }


    private bool IsAbleToChangeLane(int direction)
    {
        direction = 0;

        if (Mathf.Approximately(curPos, -1.682708f))
        {
            Debug.Log("Làn trái → chỉ có thể rẽ phải");
            Turn(dir);
            return true;
        }
        else if (Mathf.Approximately(curPos, 1.682708f))
        {
            Debug.Log("Làn phải → chỉ có thể rẽ trái");
            Turn(dir);
            return true;
        }
        else if (Mathf.Approximately(curPos, 0f))
        {
            if (direction < 0)
            {
                Debug.Log("Làn giữa → rẽ trái");
            }
            else
            {
                Debug.Log("Làn giữa → rẽ phải");
            }

            Turn(dir);


            return true;
        }

        return false;
    }

    private void Turn(int dir)
    {
        Debug.Log("+" + dir);
        // Tính chỉ số mới (trái = -1, phải = +1)
        int newIndex = Mathf.Clamp(currentIndex + dir, 0, laneX.Length - 1);

        Debug.Log(newIndex);

        /*// Lấy giá trị X mới
        float newX = laneX[newIndex];

        // Cập nhật vị trí và curPos
        transform.position = new Vector2(newX, transform.position.y);
        curPos = newX;

        Debug.Log($"🚗 Di chuyển từ làn {currentIndex} sang làn {newIndex} (x={newX})");*/
    }


    private int GetLaneIndex(float x)
    {
        for (int i = 0; i < laneX.Length; i++)
        {
            if (Mathf.Approximately(x, laneX[i]))
                return i;
        }

        return 1;
    }

    private void RandSpr()
    {
        if (sprites.Length > 0)
        {
            spr.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }

    public void Die()
    {
        anim.Play(CacheString.TAG_BROKEN);
    }

    public void SetCurPos(Vector3 spawnPos, int randIndex)
    {
        curPos = spawnPos.x;
        // Tìm chỉ số hiện tại
        currentIndex = GetLaneIndex(curPos);

        if (randIndex == 0)
        {
            // Đang ở làn trái → chỉ có thể rẽ phải
            dir = 1;
        }
        else if (randIndex == 2)
        {
            // Đang ở làn phải → chỉ có thể rẽ trái
            dir = -1;
        }
        else if (randIndex == 1)
        {
            // Đang ở giữa → có thể rẽ trái hoặc phải → random
            dir = Random.value > 0.5f ? 1 : -1;
        }
    }
}
