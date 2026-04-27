using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CurentPlayerIndicator : MonoBehaviour, IObserver
{
    public Sprite[] xSprites;
    public Sprite[] oSprites;

    private Image currentPlayerImage;
    private int theme;
    private Coroutine anim;

    void Start()
    {
        currentPlayerImage = GetComponent<Image>();
        theme = GameManager.Instance.Theme;
        SetInstant(GameManager.Instance.CurrentPlayer);

        // Observer for player changes.
        GameManager.Instance.RegisterPlayerChangeObserver(this);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterPlayerChangeObserver(this);
    }

    void SetInstant(CellState state)
    {
        currentPlayerImage.sprite =
            state == CellState.X ? xSprites[theme] : oSprites[theme];

        currentPlayerImage.color = Color.white;
        transform.localScale = Vector3.one;
    }

    public void UpdateState()
    {
        // Updates the player indicator when the player changes.
        CellState newPlayer = GameManager.Instance.CurrentPlayer;
        UpdatePlayer(newPlayer == CellState.X ? CellState.O : CellState.X);
    }

    public void UpdatePlayer(CellState lastPlayer)
    {
        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(SwitchAnimation(lastPlayer));
    }

    IEnumerator SwitchAnimation(CellState lastPlayer)
    {
        Sprite newSprite =
            lastPlayer == CellState.X ? oSprites[theme] : xSprites[theme];

        float t = 0f;
        float duration = 0.12f;

        Color originalColor = currentPlayerImage.color;
        Vector3 startScale = Vector3.one;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            currentPlayerImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f - progress);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * 0.92f, progress);
            yield return null;
        }

        currentPlayerImage.sprite = newSprite;
        t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            currentPlayerImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, progress);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.92f, Vector3.one, progress);
            yield return null;
        }

        currentPlayerImage.color = originalColor;
        transform.localScale = Vector3.one;
        anim = null;
    }
}