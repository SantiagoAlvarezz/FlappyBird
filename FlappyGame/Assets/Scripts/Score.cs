using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Xml;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    //[SerializeField] private TextMeshProUGUI _highScoreText;


    [SerializeField] private TextMeshProUGUI[] names;
    [SerializeField] private TextMeshProUGUI[] scores;

    private int _score;
    private int _bestScore;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    private void Start()
    {
        _currentScoreText.text = _score.ToString();

        GetUserScore();


    }
    public void UpdateScore()
    {
        _score++;
        _currentScoreText.text = _score.ToString();
    }

    public void SetHighestScore()
    {
        if (_bestScore > _score)
        {
            _score = _bestScore;
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).
            Child("score").SetValueAsync(_score);
        }
        else if (_bestScore < _score)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).
            Child("score").SetValueAsync(_score);
        }
        GetUsersHighestScores();
    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/score")
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string _score = "" + snapshot.Value;
                _bestScore = int.Parse(_score);

                //Debug.Log("Score: " + snapshot.Value);
            }
        });
    }

    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").OrderByChild("score").LimitToLast(3)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {
                    var userObject = ((Dictionary<string, object>)userDoc.Value);
                    
                    _currentScoreText.text = "" + userObject["score"];

                   

                    Debug.Log("LEADERBOARD: " + userObject["username"] + " : " + userObject["score"]);
                    for (int i = 0; i < ((Dictionary<string, object>)snapshot.Value).Count; i++)
                    {
                        var keyValuePair = ((Dictionary<string, object>)snapshot.Value).ElementAt(i);
                        var userValuePair = ((Dictionary<string, object>)keyValuePair.Value);
                        names[i].text = "" + userValuePair["username"];
                        scores[i].text = "" + userValuePair["score"];
                    }

                }


            }
        });
    }

}
