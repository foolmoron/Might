using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : Manager<AnimationManager> {

    [Header("UI")]
    public ButtonWithSelect[] Buttons;

    [Header("Test")]
    public FloatNear TestArm;
    public Vector2[] TestArmResting = new Vector2[4];

    void Start() {
        foreach (var buttonWithSelect in Buttons) {
            buttonWithSelect.OnSelected += button => HighlightChoice(Buttons.IndexOf(button));
        }
    }
    
    public void HighlightChoice(int choice) {
        TestArm.BaseTarget = TestArmResting[choice];
    }

    public void MakeChoice(int choice) {
        StartCoroutine(DoChoiceAnimations(choice));
    }

    IEnumerator DoChoiceAnimations(int choice) {
        Buttons.ForEach(button => button.gameObject.SetActive(false));
        Buttons[choice].gameObject.SetActive(true);
        yield return null;
    }
}