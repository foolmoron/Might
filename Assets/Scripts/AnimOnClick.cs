using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimOnClick : MonoBehaviour {

    public Animator Target;
    public string AnimToFire;
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Target.PlayFromBeginning(AnimToFire);
            enabled = false;
        }
    }
}