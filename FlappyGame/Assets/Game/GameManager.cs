using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _scoreCanvas;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);
        _scoreCanvas.SetActive(false);

        Score.instance.SetHighestScore();//aca

        Time.timeScale = 0;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
