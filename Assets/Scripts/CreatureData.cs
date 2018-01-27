using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreaturePhase {
    public CreatureAction GoodAction;
    [TextArea]
    public string GoodReaction = "The creature was damaged!";
    public CreatureAction BadAction;
    [TextArea]
    public string BadReaction = "The creature hurt you!";
    [TextArea]
    public string NeutralReaction = "Nothing happened...";
}
[Serializable]
public class Dialogue {
    [TextArea]
    public string Text = "Hi I am a creature";
    [Range(-5, 5)]
    public int HealthChange = 0;
    [Range(-500, 500)]
    public int GoldChange = 0;
}

[CreateAssetMenu(fileName = "rawr", menuName = "MIGHT/Creature", order = 0)]
public class CreatureData : ScriptableObject {
    public Difficulty Difficulty;
    public string Name;
    public Sprite Sprite;
    public List<CreaturePhase> Pattern;
    public List<Dialogue> Dialogues;
}
