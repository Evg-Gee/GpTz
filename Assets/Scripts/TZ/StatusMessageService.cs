using UnityEngine;

// Сервис показа технологических сообщений и подсказок
public class StatusMessageService : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text _messageText;
    [SerializeField] private float _displayTime = 2f;

    private Coroutine _current;

    public void ShowMessage(string message)
    {
        if (_current != null) StopCoroutine(_current);
        _current = StartCoroutine(Display(message));
    }

    private System.Collections.IEnumerator Display(string msg)
    {
        _messageText.text = msg;
        _messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(_displayTime);
        _messageText.gameObject.SetActive(false);
    }
}