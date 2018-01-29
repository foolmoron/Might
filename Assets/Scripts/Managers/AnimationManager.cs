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
    public AudioClip SelectSound;

    [Header("RPG")]
    public FloatNear ArmSword;
    public FloatNear ArmMagic;
    public FloatNear ArmDefense;
    public FloatNear ArmTalk;
    public Vector3 ArmBottom;
    public Vector3 ArmUp;
    public Vector3 ArmAct;
    public Vector3 ActionAnimPosition;
    public GameObject[] ActionAnimPrefabs;
    public AudioClip[] ActionSounds = new AudioClip[4];
    public AudioClip GoodSound;
    public AudioClip BadSound;
    public AudioClip NeutralSound;

    [Header("Test")]
    public FloatNear TestArm;
    public Vector2[] TestArmResting = new Vector2[4];
    public float ScribblingX;
    public ParticleSystem CorrectParticles;
    public ParticleSystem WrongParticles;
    public AudioClip TestCorrect;
    public AudioClip TestWrong;
    public AudioClip ScribbleSound;

    void Start() {
        foreach (var buttonWithSelect in Buttons) {
            buttonWithSelect.OnSelected += button => HighlightChoice(Buttons.IndexOf(button));
        }
        CorrectParticles.enableEmission(false);
        WrongParticles.enableEmission(false);
        ArmSword.BaseTarget = ArmUp;
    }
    
    public void HighlightChoice(int choice) {
        TestArm.BaseTarget = TestArmResting[choice].to3(TestArm.BaseTarget.z);
        ArmSword.BaseTarget = choice == 0 ? ArmUp : ArmBottom;
        ArmMagic.BaseTarget = choice == 1 ? ArmUp : ArmBottom;
        ArmDefense.BaseTarget = choice == 2 ? ArmUp : ArmBottom;
        ArmTalk.BaseTarget = choice == 3 ? ArmUp : ArmBottom;
    }

    public void MakeChoice(int choice) {
        StartCoroutine(DoChoiceAnimations(choice));
    }

    IEnumerator DoChoiceAnimations(int choice) {
        SelectSound.Play();
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
            switch (choice) {
                case 0:
                    ArmSword.BaseTarget = ArmAct;
                    break;
                case 1:
                    ArmMagic.BaseTarget = ArmAct;
                    break;
                case 2:
                    ArmDefense.BaseTarget = ArmAct;
                    break;
                case 3:
                    ArmTalk.BaseTarget = ArmAct;
                    break;
            }
            yield return new WaitForSeconds(0.8f);
            var prefab = ActionAnimPrefabs[choice];
            if (prefab) {
                Instantiate(prefab, ActionAnimPosition, Quaternion.identity);
            }
            var sound = ActionSounds[choice];
            if (sound) {
                sound.Play();
            }
            yield return new WaitForSeconds(0.2f);
            switch (choice) {
            case 0:
                ArmSword.BaseTarget = ArmUp;
                break;
            case 1:
                ArmMagic.BaseTarget = ArmUp;
                break;
            case 2:
                ArmDefense.BaseTarget = ArmUp;
                break;
            case 3:
                ArmTalk.BaseTarget = ArmUp;
                break;
            }
            TestArm.BaseTarget = TestArm.BaseTarget.withX(ScribblingX); // move test arm with RPG action
            ScribbleSound.Play();
            yield return new WaitForSeconds(0.25f);
            if (GameManager.Inst.previousCreatureReaction == 1) {
                GoodSound.Play();
            } else if (GameManager.Inst.previousCreatureReaction == -1) {
                BadSound.Play();
            } else {
                NeutralSound.Play();
            }
            yield return new WaitForSeconds(1.5f);
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
                TestCorrect.Play();
            } else {
                WrongParticles.enableEmission(true);
                TestWrong.Play();
            }
            yield return new WaitForSeconds(0.4f);
            ScribbleSound.Play();
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