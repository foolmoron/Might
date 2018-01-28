using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerps : MonoBehaviour {

    public Color[] Colors;
    public float Speed;
    public float Value;

    SpriteRenderer r;

    void Awake() {
        r = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Value += Speed * Time.deltaTime;
        Value = Value % Colors.Length;
        var colorIndex = Mathf.FloorToInt(Value);
        var nextColor = (colorIndex + 1) % Colors.Length;
        var lerp = Value % 1;
        var color = Color.Lerp(Colors[colorIndex], Colors[nextColor], lerp);
        r.color = color;
    }
}