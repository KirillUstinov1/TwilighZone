using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //allow this component to be grabbed from anywhere and make sure only one exists
    public static GameManager Instance;

    //event to listen to for the score change
    public static UnityEvent ScoreUpdate = new UnityEvent();

    //score property and int behind it
    private static int deaths = 0;
    public static int score
    {
        get
        {
            return deaths;
        }
        set
        {
            deaths = value;
            ScoreUpdate.Invoke();
        }
    }


    //when made make sure this is the only manager, and make the manager persistant through levels
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public static void ResetGame()
    {
        deaths = 0;
    }

}
