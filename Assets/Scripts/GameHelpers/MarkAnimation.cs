using UnityEngine;

/// <summary>
/// Handles the placement animation for X and O marks on the board.
/// Animates the mark scaling in from zero to its target size.
/// </summary>
public class MarkAnimation : MonoBehaviour
{
    public float animationDuration = 0.3f;

    public void PlayPlacementAnimation(float targetSize)
    {
        StartCoroutine(AnimatePlacement(targetSize));
    }

    private System.Collections.IEnumerator AnimatePlacement(float targetSize)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            yield break;

        // Calculates the target scale based on the sprite size and desired world size.
        float spriteSize = spriteRenderer.bounds.size.x;
        float targetScale = targetSize / spriteSize;

        // Starts from zero scale.
        transform.localScale = Vector3.zero;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            // Applies easing for smooth animation.
            t = EaseOut(t);
            transform.localScale = Vector3.one * targetScale * t;

            yield return null;
        }

        // Ensures the final scale is exact.
        transform.localScale = Vector3.one * targetScale;
    }

    private float EaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}