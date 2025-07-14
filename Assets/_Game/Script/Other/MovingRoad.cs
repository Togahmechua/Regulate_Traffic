using UnityEngine;

public class MovingRoad : MonoBehaviour
{
    [Header("Loop List")]
    [SerializeField] private RectTransform[] items;

    [Header("Setting Speed")]
    [SerializeField] private float speed = 100f;

    private float itemHeight;
    private float spacing = 0f;

    void Start()
    {
        if (items.Length < 2)
        {
            Debug.LogWarning("Cần ít nhất 2 item để loop!");
            return;
        }

        itemHeight = items[0].rect.height;

        // Tính khoảng cách giữa các item nếu có spacing
        spacing = Mathf.Abs(items[1].anchoredPosition.y - items[0].anchoredPosition.y) - itemHeight;
    }

    void Update()
    {
        foreach (RectTransform item in items)
        {
            Vector2 pos = item.anchoredPosition;
            pos.y -= speed * Time.deltaTime;
            item.anchoredPosition = pos;
        }

        for (int i = 0; i < items.Length; i++)
        {
            RectTransform item = items[i];

            if (item.anchoredPosition.y < -itemHeight - spacing)
            {
                // Tìm item đang ở vị trí cao nhất
                RectTransform top = GetTopMostItem();

                // Đưa item này lên trên top
                float newY = top.anchoredPosition.y + itemHeight + spacing;
                item.anchoredPosition = new Vector2(item.anchoredPosition.x, newY);

                // Đưa nó lên đầu trong hierarchy để đảm bảo hiển thị đúng thứ tự
                item.SetAsFirstSibling();
            }
        }
    }

    private RectTransform GetTopMostItem()
    {
        RectTransform top = items[0];
        foreach (RectTransform item in items)
        {
            if (item.anchoredPosition.y > top.anchoredPosition.y)
            {
                top = item;
            }
        }
        return top;
    }
}
