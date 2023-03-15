using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private int score = 0;
    private int highscore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore");
        GameManager.OnSpawingCube += GameManager_OnSpawingCube;
        GameManager.OnPlay += GameManager_OnPlay;
        GameManager.OnGameOver += GameManager_OnGameOver;
    }

    private void OnDestroy()
    {
        GameManager.OnSpawingCube -= GameManager_OnSpawingCube;
        GameManager.OnPlay -= GameManager_OnPlay;
        GameManager.OnGameOver -= GameManager_OnGameOver;

    }

    private void GameManager_OnPlay()
    {
        scoreText.text = score.ToString();
    }

    private void GameManager_OnGameOver()
    {
        endScoreText.text = score.ToString();
        highScoreText.text = highscore.ToString();
        score = 0;
        scoreText.text = score.ToString();

    }

    private void GameManager_OnSpawingCube()
    {
        score++;
        scoreText.text = score.ToString();
        if (highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("HighScore", highscore);
            highScoreText.text = highscore.ToString();
        }
    }

}
