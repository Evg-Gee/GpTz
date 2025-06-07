using UnityEngine;
using UnityEngine.UI;

public class StatusMessage : MonoBehaviour
{
    [SerializeField] private Text messageText;

    public void UpdateMessage(string message)
    {
        messageText.text = message;
    }

    public void ShowHint(string hint)
    {
        messageText.text = hint;
    }
}