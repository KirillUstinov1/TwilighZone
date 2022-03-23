using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
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
        myText.text = "" + GameManager.time1;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeText();
    }
}
