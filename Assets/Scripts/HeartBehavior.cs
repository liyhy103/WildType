using UnityEngine;
using UnityEngine.UI;

public class HeartEffectBehavior : MonoBehaviour
{
    public float lifetime = 2f;
    public float floatSpeed = 20f;
    public float fadeSpeed = 1f;
    public float shrinkSpeed = 0.5f;

    private RectTransform rectTransform;
    private Image image;
    private float timeElapsed = 0f;
    private Color startColor;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startColor = image.color;
    }

    void Update() {
        timeElapsed += Time.deltaTime;

        // upward movement
        rectTransform.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

        // Fade 
        float alpha = Mathf.Lerp(startColor.a, 0, timeElapsed / lifetime);
        image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Shrinking move
        float scale = Mathf.Lerp(1f, 0.3f, timeElapsed / lifetime);
        rectTransform.localScale = new Vector3(scale, scale, 1f);
        if (timeElapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
