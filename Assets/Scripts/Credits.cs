using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    public Text GoldText;
    public Image GradeImage;
    public Sprite[] Grades;

    void Start() {
        GoldText.text = GameManager.Gold.ToString();
        if (GameManager.Score >= 8) { // A
            GradeImage.sprite = Grades[0];
        } else if (GameManager.Score >= 4) { // B
            GradeImage.sprite = Grades[1];
        } else if (GameManager.Score >= 0) { // C
            GradeImage.sprite = Grades[2];
        } else if (GameManager.Score >= -4) { // D
            GradeImage.sprite = Grades[3];
        } else { // F
            GradeImage.sprite = Grades[4];
        }
    }

    float t;

    void Update() {
        t += Time.deltaTime;
        if (t > 2f && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("main");
        }
    }
}