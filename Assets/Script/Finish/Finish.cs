using System;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private List<Transform> _placesTargets;
    [SerializeField] private List<FinishBonusCell> _bonusCells;
    [SerializeField] private Transform _cameraLocation;

    private Dictionary<Character, int> _characterRecords = new Dictionary<Character, int>();
    private Dictionary<int, Transform> _parsedPlacesTargets = new Dictionary<int, Transform>();

    private Character _bonusLevelMember;
    private FinishBonusCell _bonusLevelMemberCell;

    private FinishBonusCell _lastPassedBonusCell;

    private int _winnerCounter = 1;
    private bool _loseBonusLevel;


    public static bool IsWin { get; private set; }

    public static int CoinsFactor { get; private set; }

    protected void Awake()
    {
        CoinsFactor = 1;
        IsWin = false;

        ParsedPlacesTargets();

        foreach (var winner in _characterRecords.Keys)
        {
            if (_parsedPlacesTargets.ContainsKey(_winnerCounter) == true)
            {
                MoveWinnerToHisPlace(winner);
            }
            else
            {
                throw new Exception($"Can't find winner place for {winner}.");
            }
        }
    }

    protected void FixedUpdate()
    {
        foreach (var winner in _characterRecords.Keys)
        {
            if (_bonusLevelMember == winner)
            {
                if (_loseBonusLevel == false)
                {
                    continue;
                }

                MoveCameraToLocation(_cameraLocation);
            }

            MoveWinnerToHisPlace(winner);
        }

        if (_bonusLevelMember == null || _bonusLevelMemberCell == null)
        {
            return;
        }

        _bonusLevelMember.transform.position = _bonusLevelMemberCell.CharacterTarget.position;
        _bonusLevelMember.transform.rotation = _bonusLevelMemberCell.CharacterTarget.rotation;

        MoveCameraToLocation(_bonusLevelMemberCell.CameraLocation);
    }

    protected void OnEnable()
    {
        foreach (var cell in _bonusCells)
        {
            cell.OnPassed += UpdateBonusCell;
        }
    }

    protected void OnDisable()
    {
        foreach (var cell in _bonusCells)
        {
            cell.OnPassed -= UpdateBonusCell;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character winner) == true)
        {
            AcceptWinner(winner);
        }
    }

    public void AcceptWinner(Character winner)
    {
        if (_characterRecords.ContainsKey(winner) == true)
        {
            return;
        }

        _characterRecords[winner] = _winnerCounter;
        _winnerCounter++;

        if (winner.TryGetComponent(out UserLogic user) == true)
        {
            if (_characterRecords[winner] <= 1)
            {
                IsWin = true;
            }

            if (_characterRecords[winner] == 1)
            {
                StartBonusLevel(winner);

                return;
            }
            else
            {
                SystemMessenger.Send("You are " + _characterRecords[winner]);

                MoveCameraToLocation(_cameraLocation);
            }
        }
        else if (winner.TryGetComponent(out HasNickname nicknameObject) == true)
        {
            SystemMessenger.Send(nicknameObject.Get() + " won!");
        }

        winner.Win();
    }

    private void MoveCameraToLocation(Transform location)
    {
        Camera.main.transform.SetParent(location);

        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localEulerAngles = Vector3.zero;
    }

    private void MoveWinnerToHisPlace(Character winner)
    {
        int placeNumber = _characterRecords[winner];
        Transform placeTarget = _parsedPlacesTargets[placeNumber];

        winner.transform.position = placeTarget.transform.position;
        winner.transform.rotation = placeTarget.transform.rotation;
    }

    private void ParsedPlacesTargets()
    {
        foreach (var target in _placesTargets)
        {
            if (int.TryParse(target.name, out int number) == true)
            {
                _parsedPlacesTargets[number] = target;
            }
            else
            {
                throw new Exception($"Can't parse target: {target}.");
            }
        }
    }

    private void StartBonusLevel(Character winner)
    {
        winner.SecondLive = true;

        foreach (var cell in _bonusCells)
        {
            cell.gameObject.SetActive(true);
        }

        _bonusLevelMember = winner;
        winner.OnDie += ToLastBonusCell;
    }

    private void ToLastBonusCell()
    {
        if (_lastPassedBonusCell == null)
        {
            _loseBonusLevel = true;
        }
        else
        {
            _bonusLevelMemberCell = _lastPassedBonusCell;
            CoinsFactor = _bonusLevelMemberCell.CoinsFactor;
        }

        _bonusLevelMember.Win();
        _bonusLevelMember.OnDie -= ToLastBonusCell;
    }

    private void UpdateBonusCell(FinishBonusCell cell)
    {
        _lastPassedBonusCell = cell;

        if (cell.IsLast == false)
        {
            return;
        }

        _bonusLevelMemberCell = cell;
        CoinsFactor = cell.CoinsFactor;

        _bonusLevelMember.Win();
        _bonusLevelMember.OnDie -= ToLastBonusCell;
    }
}