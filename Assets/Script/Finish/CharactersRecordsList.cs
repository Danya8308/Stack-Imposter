using System;
using System.Collections.Generic;
using UnityEngine;

public class CharactersRecordsList : MonoBehaviour
{
    [SerializeField] private Character _user;

    private Dictionary<Action, Character> _finishers = new Dictionary<Action, Character>();
    private Dictionary<Action, Character> _dead = new Dictionary<Action, Character>();

    private List<CharacterRecord> _records = new List<CharacterRecord>();

    private Character[] _spawnedCharacters;

    private bool _userFinished;


    private int FinishersCount => _finishers.Count;

    private void Awake()
    {
        _userFinished = false;
    }

    protected void OnEnable()
    {
        _spawnedCharacters = FindObjectsOfType<Character>();

        _user.OnWin += EnableRecords;

        foreach (var character in _spawnedCharacters)
        {
            Action addFinisher = null;
            Action addDead = null;

            addFinisher = () => AcceptAsFinisher(addFinisher, character);
            addDead = () => AcceptAsDead(addDead, character);

            character.OnWin += addFinisher;
            character.OnDie += addDead;
        }
    }

    protected void OnDisable()
    {
        _user.OnWin -= EnableRecords;

        foreach (var addFinisher in _finishers.Keys)
        {
            _finishers[addFinisher].OnWin -= addFinisher;
        }

        foreach (var addDead in _dead.Keys)
        {
            _dead[addDead].OnDie -= addDead;
        }
    }

    private void AcceptAsFinisher(Action key, Character finisher)
    {
        if (CheckOnExist(key) == true)
        {
            return;
        }

        CreateNewRecord().SetFinisher(finisher, FinishersCount + 1);
        _finishers[key] = finisher;
    }

    private void AcceptAsDead(Action key, Character dead)
    {
        if (CheckOnExist(key) == true || dead.SecondLive == true)
        {
            return;
        }

        CreateNewRecord().SetDead(dead);
        _dead[key] = dead;
    }

    private CharacterRecord CreateNewRecord()
    {
        CharacterRecord recordPrefab = Resources.Load<CharacterRecord>(CharacterRecord.PrefabPath);
        CharacterRecord record = Instantiate(recordPrefab, transform);

        record.gameObject.SetActive(_userFinished);

        _records.Add(record);

        return record;
    }

    private void EnableRecords()
    {
        _userFinished = true;

        foreach (var record in _records)
        {
            record.gameObject.SetActive(true);
        }
    }

    private bool CheckOnExist(Action key)
    {
        return _finishers.ContainsKey(key) || _dead.ContainsKey(key);
    }
}