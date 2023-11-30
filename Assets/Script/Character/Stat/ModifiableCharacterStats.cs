using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableCharacterStats
{
    [SerializeField] private CharacterStats _default;

    private List<CharacterStats> _effects = new List<CharacterStats>();


    public CharacterStats Default => _default;

    public CharacterStats Current => GetCurrent();

    public ModifiableCharacterStats(CharacterStats defaultValues)
    {
        _default = defaultValues;
    }

    public void TakeStatEffectWhile(MonoBehaviour context, CharacterStats effect, Func<bool> condition)
    {
        IEnumerator take()
        {
            _effects.Add(effect);

            yield return new WaitWhile(condition);

            _effects.Remove(effect);
        }

        context.StartCoroutine(take());
    }

    private CharacterStats GetCurrent()
    {
        if (_effects == null)
        {
            _effects = new List<CharacterStats>();
        }

        CharacterStats current = _default;

        foreach (var effect in _effects)
        {
            current += effect;
        }

        return current;
    }
}