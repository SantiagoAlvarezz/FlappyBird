using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
public class PasswordRestButton : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _emailInputField;
    private FirebaseAuth auth;
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SendPasswordResetEmail()
    {
        string emailAddress = _emailInputField.text;

        auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
        });
    }
}
