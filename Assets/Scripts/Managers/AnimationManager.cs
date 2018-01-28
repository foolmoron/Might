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
    public float ScribblingX;

    void Start() {
        foreach (var buttonWithSelect in Buttons) {
            buttonWithSelect.OnSelected += button => HighlightChoice(Buttons.IndexOf(button));
        }
    }
    
    public void HighlightChoice(int choice) {
        TestArm.BaseTarget = TestArmResting[choice].to3(TestArm.BaseTarget.z);
    }

    public void MakeChoice(int choice) {
        StartCoroutine(DoChoiceAnimations(choice));
    }

    IEnumerator DoChoiceAnimations(int choice) {
        // disable buttons
        {
            Buttons.ForEach(button => button.gameObject.SetActive(false));
            Buttons.ForEach(button => button.interactable = false);
            Buttons[choice].gameObject.SetActive(true);
        }
        // do rpg action
        {
            
        }
        // do test scribble
        {
            TestArm.BaseTarget = TestArm.BaseTarget.withX(ScribblingX);
            yield return new WaitForSeconds(1.25f);
            TestArm.BaseTarget = TestArmResting[choice].to3(TestArm.BaseTarget.z);
            yield return new WaitForSeconds(0.5f);
        }
        // reset buttons
        {
            Buttons.ForEach(button => button.gameObject.SetActive(true));
            Buttons.ForEach(button => button.interactable = true);
        }
        // 
        GameManager.Inst.NewRound();
    }
}