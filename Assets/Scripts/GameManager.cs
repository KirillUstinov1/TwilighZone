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
    private static float Timer;
    private static float lastTime = 0;
    private static int lastDeaths = 0;
    public static bool playing = true;
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

    public static string time1
    {
        get
        {
            int minutes = Mathf.FloorToInt(Timer / 60F);
            int seconds = Mathf.FloorToInt(Timer % 60F);
            int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
            return (minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00"));
        }
        set
        {
            
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
        if (playing)
        {
            Timer += Time.deltaTime;
        }
        
    }

    public static void ResetGame()
    {
        deaths = lastDeaths-1;
        Timer = lastTime - 0.8f;
        Debug.Log(Timer);
    }

    public static void NewLevel() {
        lastTime = Timer;
        lastDeaths = deaths;
    }
}
