using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : Manager<AnimationManager> {

    [Header("UI")]
    public ButtonWithSelect[] Buttons;
    public ScrambleParams Background;
    public Vector3 BackgroundCalmParams;
    public Vector3 BackgroundActionParams;
    public ParticleSystem MagicParticles;
    public ParticleSystem DefenseParticles;

    [Header("Test")]
    public FloatNear TestArm;
    public Vector2[] TestArmResting = new Vector2[4];
    public float ScribblingX;
    public ParticleSystem CorrectParticles;
    public ParticleSystem WrongParticles;

    void Start() {
        foreach (var buttonWithSelect in Buttons) {
            buttonWithSelect.OnSelected += button => HighlightChoice(Buttons.IndexOf(button));
        }
        MagicParticles.enableEmission(false);
        DefenseParticles.enableEmission(false);
        CorrectParticles.enableEmission(false);
        WrongParticles.enableEmission(false);
    }
    
    public void HighlightChoice(int choice) {
        TestArm.BaseTarget = TestArmResting[choice].to3(TestArm.BaseTarget.z);
        MagicParticles.enableEmission(choice == 1);
        DefenseParticles.enableEmission(choice == 2);
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
        // scramble params action
        {
            Background.Width = BackgroundActionParams.x;
            Background.Frequency = BackgroundActionParams.y;
            Background.Timescale = BackgroundActionParams.z;
        }
        // do rpg action
        {
            GameManager.Inst.CreatureResponseBox.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            TestArm.BaseTarget = TestArm.BaseTarget.withX(ScribblingX); // move test arm with RPG action
            yield return new WaitForSeconds(2f);
            GameManager.Inst.CreatureResponseBox.SetActive(false);
            if (GameManager.Inst.creaturePatternDone) {
                GameManager.Inst.currentCreatureObj.GetComponent<FloatNear>().BaseTarget = GameManager.Inst.CreaturePositionFinal;
                yield return new WaitForSeconds(1f);
                Destroy(GameManager.Inst.currentCreatureObj);
            }
        }
        // do test scribble
        {
            if (GameManager.Inst.previousTestAnswerScore >= 1) {
                CorrectParticles.enableEmission(true);
            } else {
                WrongParticles.enableEmission(true);
            }
            yield return new WaitForSeconds(0.4f);
            TestArm.BaseTarget = TestArmResting[choice].to3(TestArm.BaseTarget.z);
            yield return new WaitForSeconds(1.2f);
            CorrectParticles.enableEmission(false);
            WrongParticles.enableEmission(false);
        }
        // scramble params calm
        {
            Background.Width = BackgroundCalmParams.x;
            Background.Frequency = BackgroundCalmParams.y;
            Background.Timescale = BackgroundCalmParams.z;
        }
        // reset buttons
        {
            Buttons.ForEach(button => button.gameObject.SetActive(true));
            Buttons.ForEach(button => button.interactable = true);
            Buttons[choice].Select();
        }
        // 
        GameManager.Inst.NewRound();
    }
}