using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OnWinMenu : MonoBehaviour
{
    [Header("Text:")]

    [SerializeField] private string NextLevelText = "Next";
    [SerializeField] private string RestartLevelText = "Restart";

    [Header("Components:")]

    [SerializeField] private Character _character;
    [SerializeField] private Button _completeButton;
    [SerializeField] private TextMeshProUGUI _completeButtonText;
    [SerializeField] private List<GameObject> _launchObjects;


    protected void OnEnable()
    {
        _character.OnWin += EnableObjects;
        _character.OnWin += UpdateButtonText;
        _completeButton.onClick.AddListener(CompleteEvent);
    }

    protected void OnDisable()
    {
        _character.OnWin -= EnableObjects;
        _character.OnWin -= UpdateButtonText;
        _completeButton.onClick.RemoveListener(CompleteEvent);
    }

    private void EnableObjects()
    {
        foreach (var item in _launchObjects)
        {
            item.SetActive(true);
        }
    }

    private void CompleteEvent()
    {
        if (Finish.IsWin == true)
        {
            Level.Next();
        }
        else
        {
            Level.Restart();
        }
    }

    private void UpdateButtonText()
    {
        _completeButtonText.text = Finish.IsWin == true ? NextLevelText : RestartLevelText;
    }
}