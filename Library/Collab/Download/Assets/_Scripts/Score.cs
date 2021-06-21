using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

   static int currentScore, highScore;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void IncreaseScore(int increment)
    {
        currentScore += increment;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Highscore", currentScore);
    }

    public void LoadScore()
    {
        PlayerPrefs.GetInt("Highscore", highScore);
    }
}
