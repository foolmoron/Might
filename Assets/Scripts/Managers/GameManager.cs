using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Config")]
    public CreatureData[] Creatures;
    public QuestionData[] Questions;
    [Range(0, 100)]
    public int QuestionLineLength = 25;
    [Range(0, 100)]
    public int AnswerLineLength = 22;

    [Header("RPG Stuff")]
    public int ScoreRPG;
    public Text ScoreRPGText;
    public TextMesh CreatureNameText;
    public TextMesh CreatureResponseText;
    public SpriteRenderer CreatureSprite;
    public TextMesh CreatureDebug;

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
    int currentCreaturePhase;
    int currentCreatureDialogue;
    QuestionData currentQuestion;
    int[] questionOrder = {1, 2, 3, 4};

    void Start() {
        Creatures.Shuffle();
        Questions.Shuffle();
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
            ScoreRPG++;
            currentCreaturePhase++;
        } else if (creatureAction == phase.BadAction) {
            CreatureResponseText.text = phase.BadReaction;
            ScoreRPG--;
            currentCreaturePhase++;
        } else if (creatureAction == CreatureAction.Talk) {
            ScoreRPG += currentCreature.Dialogues.Count > currentCreatureDialogue 
                ? currentCreature.Dialogues[currentCreatureDialogue].Score 
                : 0
                ;
            CreatureResponseText.text = currentCreature.Dialogues[currentCreatureDialogue].Text;
            currentCreatureDialogue = Mathf.Min(currentCreatureDialogue + 1, currentCreature.Dialogues.Count - 1);
        } else {
            CreatureResponseText.text = phase.NeutralReaction;
            currentCreaturePhase++;
        }
        // do question answers
        var answer =
        ScoreTest += currentQuestion.GetAnswer(questionAnswer).Score;
        // new round
        NewRound();
    }

    public void NewRound() {
        // update texts
        ScoreRPGText.text = ScoreRPG.ToString();
        ScoreTestText.text = ScoreTest.ToString();
        // set new creature
        if (currentCreature == null || currentCreaturePhase >= currentCreature.Pattern.Count) {
            currentCreature = Creatures.Next(currentCreature);
            currentCreaturePhase = 0;
            currentCreatureDialogue = 0;
            CreatureNameText.text = currentCreature.Name;
            CreatureResponseText.text = "";
            // CreatureSprite.sprite = currentCreature.Sprite;
            CreatureDebug.text = currentCreature.name;
        }
        // set new question
        questionOrder.Shuffle();
        currentQuestion = Questions.Next(currentQuestion);
        QuestionText.text = currentQuestion.Question;
        Answer1Text.text = currentQuestion.GetAnswer(questionOrder[0]).Text;
        Answer2Text.text = currentQuestion.GetAnswer(questionOrder[1]).Text;
        Answer3Text.text = currentQuestion.GetAnswer(questionOrder[2]).Text;
        Answer4Text.text = currentQuestion.GetAnswer(questionOrder[3]).Text;
        QuestionDebug.text = currentQuestion.name;
    }
}