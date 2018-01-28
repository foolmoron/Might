using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour {

    public Vector2 Speed;
    [Range(0, 1)]
    public float OffsetX;
    [Range(0, 1)]
    public float OffsetY;

    Renderer r;
    MaterialPropertyBlock props;

    void Awake() {
        r = GetComponent<Renderer>();
        props = new MaterialPropertyBlock();
        r.GetPropertyBlock(props);
    }

    void Update() {
        OffsetX = (OffsetX + Speed.x * Time.deltaTime + 1) % 1;
        OffsetY = (OffsetY + Speed.y * Time.deltaTime + 1) % 1;
        props.SetFloat("_OffsetX", OffsetX);
        props.SetFloat("_OffsetY", OffsetY);
        r.SetPropertyBlock(props);
    }
}