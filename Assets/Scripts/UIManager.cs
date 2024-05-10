using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    
    public Text scoreText;
    public Text highScoreText;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
        // Actualiza el texto del score al iniciar el juego
        UpdateScoreText(PlayerPrefs.GetInt("Score", 0));
        UpdateHighScoreText(PlayerPrefs.GetInt("HighScore", 0));
    }

   
    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score " + score.ToString();
    }

    public void UpdateHighScoreText(int highScore)
    {
        highScoreText.text = "High "+highScore.ToString();
    }
}
