using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorThenDie : MonoBehaviour {

    public Color ColorStart = Color.black;
    public Color ColorEnd = Color.black.withAlpha(0);
    public float Duration;

    void Start() {
        var r = GetComponent<Image>();
        r.enabled = true;
        r.color = ColorStart;
        Tween.ColorTo(gameObject, ColorEnd, Duration, Interpolate.EaseType.Linear);
        Destroy(gameObject, Duration * 1.05f);
    }
}