using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Manager<GameManager> {

    [Header("Config")]
    [Range(0, 100)]
    public int QuestionLineLength = 25;
    [Range(0, 100)]
    public int AnswerLineLength = 22;
    [Range(0, 100)]
    public int ResponseLineLength = 23;
    [Range(0, 100)]
    public int QuestionsBeforeWin = 12;
    int questionsAnswered;

    public bool isWin { get { return questionsAnswered >= QuestionsBeforeWin; } }
    public bool isLose { get { return HealthRPG == 0; } }

    public static int Gold;
    public static int Score;

    [Header("RPG Stuff")]
    public int HealthRPG = 5;
    public GameObject[] Hearts;
    public int GoldRPG;
    public Text GoldRPGText;
    public TextMesh CreatureNameText;
    public GameObject CreatureResponseBox;
    public TextMesh CreatureResponseText;
    public TextMesh CreatureDebug;
    public Vector3 CreaturePosition;
    public Vector3 CreaturePositionFinal;
    public Vector3 CreatureScaleInit = Vector3.zero;
    public Vector3 CreatureScaleFinal = Vector3.one;

    [Header("Test Stuff")]
    public int ScoreTest;
    public Image GradeImage;
    public Sprite[] Grades;
    public TextMesh QuestionText;
    public TextMesh Answer1Text;
    public TextMesh Answer2Text;
    public TextMesh Answer3Text;
    public TextMesh Answer4Text;
    public TextMesh QuestionDebug;

    public CreatureData currentCreature;
    public GameObject currentCreatureObj { get; private set; }
    int currentCreaturePhase;
    int currentCreatureDialogue;
    public int previousCreatureReaction { get; private set; }
    public bool creaturePatternDone { get { return currentCreaturePhase >= currentCreature.Pattern.Count; } }
    public bool creatureDialogueDone { get { return currentCreatureDialogue >= currentCreature.Dialogues.Count; } }
    QuestionData currentQuestion;
    int[] questionOrder = {1, 2, 3, 4};
    public int previousTestAnswerScore { get; private set; }

    void Start() {
        AssetManager.Inst.Creatures.Shuffle();
        AssetManager.Inst.Questions.Shuffle();
        NewRound();
    }

    public void SelectAction(int choice) {
        // figure out choices
        var creatureAction = CreatureAction.Talk;
        var questionAnswer = 1;
        switch (choice) {
            case 1:
                creatureAction = CreatureAction.Attack;
                questionAnswer = questionOrder[0];
                break;
            case 2:
                creatureAction = CreatureAction.Magic;
                questionAnswer = questionOrder[1];
                break;
            case 3:
                creatureAction = CreatureAction.Defend;
                questionAnswer = questionOrder[2];
                break;
            case 4:
                creatureAction = CreatureAction.Talk;
                questionAnswer = questionOrder[3];
                break;
        }
        // do creature action
        var phase = currentCreature.Pattern[currentCreaturePhase];
        previousCreatureReaction = 0;
        if (creatureAction == phase.GoodAction) {
            CreatureResponseText.text = phase.GoodReaction.LineWrap(ResponseLineLength);
            HealthRPG++;
            GoldRPG += phase.GoldOnGood;
            currentCreaturePhase++;
            previousCreatureReaction = 1;
        } else if (creatureAction == phase.BadAction) {
            CreatureResponseText.text = phase.BadReaction.LineWrap(ResponseLineLength);
            HealthRPG--;
            currentCreaturePhase++;
            previousCreatureReaction = -1;
        } else if (phase.UseBad2 && creatureAction == phase.BadAction2) {
            CreatureResponseText.text = phase.BadReaction2.LineWrap(ResponseLineLength);
            HealthRPG--;
            currentCreaturePhase++;
        } else if (creatureAction == CreatureAction.Talk) {
            HealthRPG += currentCreature.Dialogues.Count > currentCreatureDialogue
                ? currentCreature.Dialogues[currentCreatureDialogue].HealthChange
                : 0
                ;
            GoldRPG += currentCreature.Dialogues.Count > currentCreatureDialogue
                ? currentCreature.Dialogues[currentCreatureDialogue].GoldChange
                : 0
                ;
            CreatureResponseText.text = currentCreature.Dialogues[currentCreatureDialogue].Text.LineWrap(ResponseLineLength);
            currentCreatureDialogue = Mathf.Min(currentCreatureDialogue + 1, currentCreature.Dialogues.Count - 1);
        } else {
            CreatureResponseText.text = phase.NeutralReaction.LineWrap(ResponseLineLength);
            currentCreaturePhase++;
        }
        HealthRPG = Mathf.Clamp(HealthRPG, 0, Hearts.Length);
        GoldRPG = Mathf.Max(GoldRPG, 0);
        // do question answers
        previousTestAnswerScore = currentQuestion.GetAnswer(questionAnswer).Score;
        ScoreTest += previousTestAnswerScore;
        // endgame?
        questionsAnswered++;
        // animation
        AnimationManager.Inst.MakeChoice(choice - 1);
    }

    public void UpdateRPG() {
        for (int i = 0; i < Hearts.Length; i++) {
            Hearts[i].SetActive(i < HealthRPG);
        }
        GoldRPGText.text = GoldRPG.ToString();
    }

    public void UpdateTest() {
        if (ScoreTest >= 8) { // A
            GradeImage.sprite = Grades[0];
        } else if (ScoreTest >= 4) { // B
            GradeImage.sprite = Grades[1];
        } else if (ScoreTest >= 0) { // C
            GradeImage.sprite = Grades[2];
        } else if (ScoreTest >= -4) { // D
            GradeImage.sprite = Grades[3];
        } else { // F
            GradeImage.sprite = Grades[4];
        }
    }

    public void NewRound() {
        UpdateRPG();
        UpdateTest();
        // set new creature
        if (currentCreature == null || creaturePatternDone) {
            currentCreature = AssetManager.Inst.Creatures.Next(currentCreature);
            currentCreaturePhase = 0;
            currentCreatureDialogue = 0;
            CreatureNameText.text = currentCreature.Name;
            CreatureResponseBox.SetActive(false);
            CreatureResponseText.text = "";
            CreatureDebug.text = currentCreature.name;
            currentCreature.IntroSound.Play();
            currentCreatureObj = Instantiate(currentCreature.Prefab, CreaturePosition, Quaternion.identity);
            currentCreatureObj.transform.localScale = CreatureScaleInit;
            Tween.ScaleTo(currentCreatureObj, CreatureScaleFinal, 0.5f, Interpolate.EaseType.EaseOutQuad);
            var sprite = currentCreatureObj.GetComponentInChildren<SpriteRenderer>();
            sprite.color = sprite.color.withAlpha(0);
            Tween.ColorTo(sprite.gameObject, sprite.color.withAlpha(1), 0.4f, Interpolate.EaseType.EaseOutSine);
        }
        // set new question
        questionOrder.Shuffle();
        currentQuestion = AssetManager.Inst.Questions.Next(currentQuestion);
        QuestionText.text = currentQuestion.Question.LineWrap(QuestionLineLength);
        Answer1Text.text = currentQuestion.GetAnswer(questionOrder[0]).Text.LineWrap(AnswerLineLength);
        Answer2Text.text = currentQuestion.GetAnswer(questionOrder[1]).Text.LineWrap(AnswerLineLength);
        Answer3Text.text = currentQuestion.GetAnswer(questionOrder[2]).Text.LineWrap(AnswerLineLength);
        Answer4Text.text = currentQuestion.GetAnswer(questionOrder[3]).Text.LineWrap(AnswerLineLength);
        QuestionDebug.text = currentQuestion.name;
    }
}