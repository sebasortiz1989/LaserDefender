using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // if you are using TMP then you have to use TMP

public class ScoreDisplay : MonoBehaviour
{
    Text scoreText;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameSession.GetScore().ToString();
    }
}
