using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Answer {
    [TextArea]
    public string Text = "42";
    [Range(-1, 1)]
    public int Score = -1;
}

[CreateAssetMenu(fileName = "what", menuName = "MIGHT/Question", order = 1)]
public class QuestionObject : ScriptableObject {
    [TextArea]
    public string Question = "What is the meaning of life?";
    public Answer Answer1;
    public Answer Answer2;
    public Answer Answer3;
    public Answer Answer4;
}
