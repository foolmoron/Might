using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrambleParams : MonoBehaviour {

    public float Width;
    float width;
    public float Frequency;
    float frequency;
    public float Timescale;
    float timescale;
    [Range(0, 0.25f)]
    public float Speed = 0.1f;
    float time;

    Renderer r;
    Material m;

    void Awake() {
        r = GetComponent<Renderer>();
        m = new Material(r.sharedMaterial);
        r.material = m;

        Width = width = m.GetFloat("_Width");
        Frequency = frequency = m.GetFloat("_Freq");
        Timescale = timescale = m.GetFloat("_Timescale");
    }

    void FixedUpdate() {
        width = Mathf.Lerp(width, Width, Speed);
        frequency = Mathf.Lerp(frequency, Frequency, Speed);
        timescale = Mathf.Lerp(timescale, Timescale, Speed);
        time += Time.deltaTime * timescale;
        m.SetFloat("_Width", width);
        m.SetFloat("_Freq", frequency);
        m.SetFloat("_Timescale", timescale);
        m.SetFloat("_T", time);
    }
}