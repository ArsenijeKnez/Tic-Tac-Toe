using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationPopup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText;

    [Header("Animation")]
    public float slideDuration = 0.3f;
    public float notificationTime = 2f;
    public float margin = 30f;

    private RectTransform rect;
    private Coroutine currentRoutine;

    private float visibleX;
    private float hiddenX;
    private float fixedY;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        fixedY = rect.anchoredPosition.y;
        CalculatePositions();
        rect.anchoredPosition = new Vector2(hiddenX, fixedY);
    }

    void CalculatePositions()
    {
        float width = rect.rect.width;
        visibleX = 0;
        hiddenX = width + margin;
    }

    public void Show(string message)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            rect.anchoredPosition = new Vector2(hiddenX, fixedY);
        }

        messageText.text = message;
        currentRoutine = StartCoroutine(PlayNotification());
    }

    IEnumerator PlayNotification()
    {
        yield return SlideX(hiddenX, visibleX);
        yield return new WaitForSeconds(notificationTime);
        yield return SlideX(visibleX, hiddenX);
    }

    IEnumerator SlideX(float fromX, float toX)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float progress = EaseOut(elapsed / slideDuration);
            float x = Mathf.Lerp(fromX, toX, progress);
            rect.anchoredPosition = new Vector2(x, fixedY);
            yield return null;
        }

        rect.anchoredPosition = new Vector2(toX, fixedY);
    }

    float EaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}