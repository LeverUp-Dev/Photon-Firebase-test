using UnityEngine;
using UnityEngine.UI;

public class PlayerNameText : MonoBehaviour
{
    private Text nameText;

    private void Start()
    {
        nameText = GetComponent<Text>();

        if (AuthManager.User != null)
        {
            nameText.text = "Hi! " + AuthManager.User.Email.ToString();
        }
        else
        {
            nameText.text = "ERROR : Not Find The User";
        }
    }
}
