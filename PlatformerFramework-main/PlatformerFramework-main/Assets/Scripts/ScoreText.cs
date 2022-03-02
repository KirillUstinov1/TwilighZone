using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TMP_Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TMP_Text>();
        ChangeText();
        GameManager.ScoreUpdate.AddListener(ChangeText);
    }

    private void ChangeText()
    {
        myText.text = "Score: " + GameManager.score;
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
