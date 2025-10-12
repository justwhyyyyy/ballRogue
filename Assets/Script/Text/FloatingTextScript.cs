using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float lifetime = 1f;
    public Vector3 moveDirection = Vector3.up;

    public float maxFontSize = 20;
    public float minFontSize = 4;

    private TextMeshPro text;
    private float fadeTime;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        fadeTime = lifetime;
    }

    public void SetText(int damage,Color color)
    {
        text.text = damage.ToString();
        text.color = color;
        float transition = (Mathf.Clamp(damage, 10f, 100f) - 10) / 90f;
        text.fontSize = Mathf.Lerp(minFontSize, maxFontSize, transition);
    }
    void Update()
    {
        transform.position += moveDirection * floatSpeed * Time.deltaTime;
        lifetime -= Time.deltaTime;

        // ½¥Òþ
        if (lifetime < fadeTime * 0.5f)
        {
            Color c = text.color;
            c.a = Mathf.Lerp(0, 1, lifetime / (fadeTime * 0.5f));
            text.color = c;
        }

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
