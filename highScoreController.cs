using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HighScore
{
    public string playername;
    public float playtime;

    public HighScore(string pname,float ptime)
    {
        playername = pname;
        playtime = ptime;
    }
}


public class highScoreController : MonoBehaviour
{
    // Start is called before the first frame update
    List<HighScore> myHighScores;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        myHighScores = new List<HighScore>();

        myHighScores.Add(new HighScore("Gerry", 65.5f));
        myHighScores.Add(new HighScore("Joey", 60.5f));

    }

    void DisplayList()
    {
        foreach (HighScore s in myHighScores)
        {
            Debug.Log(s.playername + " " + s.playtime);
        }
    }

    void SaveList()
    {
        string[] names = new string[myHighScores.Count];

        float[] playertimes = new float[myHighScores.Count];

        int counter = 0;
        foreach (HighScore s in myHighScores)
        {
            names[counter] = s.playername;
            playertimes[counter] = s.playtime;
            counter++;
        }


        PlayerPrefsX.SetStringArray("PlayerNames",names);
        PlayerPrefsX.SetFloatArray("PlayerTimes", playertimes);

    }


    void LoadList()
    {
        string[] names;

        float[] playertimes;

        names = PlayerPrefsX.GetStringArray("PlayerNames");
        playertimes = PlayerPrefsX.GetFloatArray("PlayerTimes");


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("SceneB");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("SceneA");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveList();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadList();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayList();
        }
    }
}
