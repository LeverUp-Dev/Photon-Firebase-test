using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;

    public static FirebaseApp firebaseApp; //파이어베이스 어플리케이션 전체를 관리
    public static FirebaseAuth firebaseAuth; //파이어베이스의 어플리케이션 사용자를 관리

    public static FirebaseUser User; //입력한 이메일 정보와 패스워드 정보에 대응되는 사용자를 가져오는 역활 -> 처음은 null

    public void Start()
    {
        signInButton.interactable = false; //회원가입 버튼의 상호작용을 비활성화

        // 파이어 베이스를 구동 가능한 환경인지를 검사, 만약 디펜더시가 없다면 자동으로 연동해 준다
        //ContinueWith()는 위 작업이 실행된 후에 실행되는 메서드이며 람다식을 통해 실행시킬 수 있다 
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var result = task.Result;

                if(result != DependencyStatus.Available) //만약 디펜더시 구동이 가능한 상태가 아니면...
                {
                    Debug.LogError(result.ToString());
                    IsFirebaseReady = false;
                }
                else
                {
                    IsFirebaseReady = true;

                    firebaseApp = FirebaseApp.DefaultInstance; //firebaseApp 변수를 FirebaseApp의 전체적인 기능 관리하는 객체를 할당해 준다.
                    firebaseAuth = FirebaseAuth.DefaultInstance; //사용자를 집중적으로 관리하는 객체 할당
                }

                signInButton.interactable = IsFirebaseReady;
            }
        );
    }

    void Update()
    {
        if (emailField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordField.Select();
            }
        }
        else if (passwordField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                emailField.Select();
            }
        }
    }

    public void SignIn()
    {
        if(!IsFirebaseReady || IsSignInOnProgress || User != null) //파이어 베이스가 준비 되지 않았다면....
        {
            return;
        }

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task => {
                Debug.Log($"sign in status : {task.Status}");

                IsSignInOnProgress = false;
                signInButton.interactable = true;

                if (task.IsFaulted) //task에 문제가 있을 시
                {
                    Debug.LogError(task.Exception.ToString());
                }
                else if (task.IsCanceled) //task가 중단됬을 때
                {
                    Debug.LogError("Sign-in canceled");
                }
                else
                {
                    User = task.Result.User;
                    Debug.Log(User.Email.ToString());
                    SceneManager.LoadScene("Lobby");
                }
            }
        );
    }
}