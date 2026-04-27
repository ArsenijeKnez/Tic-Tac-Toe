using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThemeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject frameBG;
    public RectTransform xImage;
    public int themeIndex;

    private Vector3 originalScale;
    public float hoverScale = 1.2f;
    private Coroutine scaleRoutine;

    void Start()
    {
        originalScale = xImage.localScale;
        frameBG.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartScale(originalScale * hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartScale(originalScale);
    }

    void StartScale(Vector3 target)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleTo(target));
    }

    IEnumerator ScaleTo(Vector3 target)
    {
        float time = 0f;
        float duration = 0.15f;
        Vector3 start = xImage.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            xImage.localScale = Vector3.Lerp(start, target, time / duration);
            yield return null;
        }

        xImage.localScale = target;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MainMenuUI.Instance.SelectTheme(this);
    }

    public void SetSelected(bool selected)
    {
        frameBG.SetActive(selected);
    }
}