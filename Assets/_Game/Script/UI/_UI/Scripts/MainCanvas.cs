using EasyTextEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private GameObject bloodyScreen;
    [SerializeField] private TextEffect timerEff;

    private float elapsedTime = 0f;
    private Coroutine bloodyCoroutine;

    private void OnEnable()
    {
        UIManager.Ins.mainCanvas = this;
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerTxt.text = $"{minutes:00}:{seconds:00}";
        timerEff.Refresh();
    }

    public void RefreshTimer()
    {
        elapsedTime = 0f;
    }

    public void Hit()
    {
        // Nếu đang có coroutine máu đang chạy → dừng lại trước
        if (bloodyCoroutine != null)
        {
            StopCoroutine(bloodyCoroutine);
            bloodyCoroutine = null;
        }

        // Bắt đầu hiệu ứng mới và lưu lại coroutine
        bloodyCoroutine = StartCoroutine(BloodyScreenEffect());
    }
    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(true);

        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float t = 0f;

        while (t < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            t += Time.deltaTime;
            yield return null;
        }

        if (bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(false);

        // ✅ Xóa biến coroutine sau khi xong
        bloodyCoroutine = null;
    }

    public void ResetUI()
    {
        // Đặt lại thời gian và cập nhật text
        elapsedTime = 0f;
        timerTxt.text = "00:00";
        timerEff.Refresh();

        // Dừng hiệu ứng máu nếu có
        if (bloodyCoroutine != null)
        {
            StopCoroutine(bloodyCoroutine);
            bloodyCoroutine = null;
        }

        // Ẩn hiệu ứng máu nếu đang hiện
        if (bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(false);
    }
}
