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

    [Header("RPG Stuff")]
    public int HealthRPG = 10;
    public Text HealthRPGText;
    public int GoldRPG;
    public Text GoldRPGText;
    public TextMesh CreatureNameText;
    public TextMesh CreatureResponseText;
    public SpriteRenderer CreatureSprite;
    public TextMesh CreatureDebug;
    public Vector3 CreaturePositionInit;
    public Vector3 CreaturePositionFinal;

    [Header("Test Stuff")]
    public int ScoreTest;
    public Text ScoreTestText;
    public TextMesh QuestionText;
    public TextMesh Answer1Text;
    public TextMesh Answer2Text;
    public TextMesh Answer3Text;
    public TextMesh Answer4Text;
    public TextMesh QuestionDebug;

    CreatureData currentCreature;
    public GameObject currentCreatureObj { get; private set; }
    int currentCreaturePhase;
    int currentCreatureDialogue;
    public bool creaturePatternDone { get { return currentCreaturePhase >= currentCreature.Pattern.Count; } }
    public bool creatureDialogueDone { get { return currentCreatureDialogue >= currentCreature.Dialogues.Count; } }
    QuestionData currentQuestion;
    int[] questionOrder = {1, 2, 3, 4};

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
        if (creatureAction == phase.GoodAction) {
            CreatureResponseText.text = phase.GoodReaction;
            HealthRPG++;
            currentCreaturePhase++;
        } else if (creatureAction == phase.BadAction) {
            CreatureResponseText.text = phase.BadReaction;
            HealthRPG--;
            currentCreaturePhase++;
        } else if (phase.UseBad2 && creatureAction == phase.BadAction2) {
            CreatureResponseText.text = phase.BadReaction2;
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
            CreatureResponseText.text = currentCreature.Dialogues[currentCreatureDialogue].Text;
            currentCreatureDialogue = Mathf.Min(currentCreatureDialogue + 1, currentCreature.Dialogues.Count - 1);
        } else {
            CreatureResponseText.text = phase.NeutralReaction;
            currentCreaturePhase++;
        }
        // do question answers
        ScoreTest += currentQuestion.GetAnswer(questionAnswer).Score;
        // animation
        AnimationManager.Inst.MakeChoice(choice - 1);
    }

    public void NewRound() {
        // update texts
        HealthRPGText.text = HealthRPG.ToString();
        GoldRPGText.text = GoldRPG.ToString();
        ScoreTestText.text = ScoreTest.ToString();
        // set new creature
        if (currentCreature == null || creaturePatternDone) {
            currentCreature = AssetManager.Inst.Creatures.Next(currentCreature);
            currentCreaturePhase = 0;
            currentCreatureDialogue = 0;
            CreatureNameText.text = currentCreature.Name;
            CreatureResponseText.text = "";
            CreatureDebug.text = currentCreature.name;
            currentCreatureObj = Instantiate(currentCreature.Prefab, CreaturePositionInit, Quaternion.identity);
            currentCreatureObj.GetComponent<FloatNear>().BaseTarget = CreaturePositionFinal;
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