using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class OnDieMenu : MonoBehaviour
{
    [SerializeField] private Character _character;

    [SerializeField] private Button _restartButton;

    [SerializeField] private List<GameObject> _launchObjects;


    protected void OnEnable()
    {
        _character.OnDie += EnableObjects;
        _restartButton.onClick.AddListener(Level.Restart);
    }

    protected void OnDisable()
    {
        _character.OnDie -= EnableObjects;
        _restartButton.onClick.RemoveListener(Level.Restart);
    }

    private void EnableObjects()
    {
        if (_character.SecondLive == true)
        {
            return;
        }

        foreach (var item in _launchObjects)
        {
            item.SetActive(true);
        }
    }
}