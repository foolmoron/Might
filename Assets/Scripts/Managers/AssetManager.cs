using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class AssetManager : Manager<AssetManager> {
    
    public CreatureData[] Creatures;
    public QuestionData[] Questions;

    void Awake() {
#if UNITY_EDITOR
        Creatures = AssetDatabase.FindAssets("t:CreatureData").Map(guid => AssetDatabase.LoadAssetAtPath<CreatureData>(AssetDatabase.GUIDToAssetPath(guid)));
        Questions = AssetDatabase.FindAssets("t:QuestionData").Map(guid => AssetDatabase.LoadAssetAtPath<QuestionData>(AssetDatabase.GUIDToAssetPath(guid)));
#endif
        Creatures.Shuffle();
        Questions.Shuffle();
    }
}