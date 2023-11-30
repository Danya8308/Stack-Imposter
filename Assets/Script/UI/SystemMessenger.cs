using System.Collections;
using UnityEngine;
using TMPro;

public class SystemMessenger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _UIText;

    private static IEnumerator _sendCoroutine;


    public static SystemMessenger Instance { get; private set; }

    public static void Send(string text, float seconds = 2f)
    {
        if (Instance is null)
        {
            return;
        }

        IEnumerator send()
        {
            Instance._UIText.text = text;

            yield return new WaitForSeconds(seconds);

            Instance._UIText.text = "";
        }

        if (_sendCoroutine is not null)
        {
            Instance.StopCoroutine(_sendCoroutine);
        }

        _sendCoroutine = send();
        Instance.StartCoroutine(_sendCoroutine);
    }

    protected void Awake()
    {
        Instance = this;
    }
}