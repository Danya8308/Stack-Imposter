using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterObjectsSwitchLogic : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _scripts;
    [SerializeField] private List<GameObject> _objects;

    private Character _character;


    protected void Awake()
    {
        _character = GetComponent<Character>();

        Disable();
    }

    protected void OnEnable()
    {
        Level.OnStart += Enable;
        _character.OnWin += Disable;
    }

    protected void OnDisable()
    {
        Level.OnStart -= Enable;
        _character.OnWin -= Disable;

        Disable();
    }

    private void Enable()
    {
        SetActive(true);
    }

    private void Disable()
    {
        SetActive(false);
    }

    private void SetActive(bool state)
    {
        foreach (var item in _objects)
        {
            item.SetActive(state);
        }

        foreach (var item in _scripts)
        {
            item.enabled = state;
        }
    }
}