using UnityEngine;

public class BotLogic : CharacterLogic
{
    [Space(5f)]

    [SerializeField] private ShiftToPoint _shifter;
    [SerializeField] private BotType _type;

    private int _currentPointIndex;


    public Transform CurrentPoint => AllBotMovePoints.Instance == null ? null : AllBotMovePoints.Instance.GetPoint(_type, _currentPointIndex);

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (CachedCharacter.IsJumping == false && CachedCharacter.OnSurface == true && CurrentPoint != null)
        {
            _shifter.Launch(this);
        }
        else if (_shifter.Active == true)
        {
            _shifter.Stop();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _shifter.OnStop += TryIncrementCurrentPointIndex;
        CachedCharacter.OnTakeStatEffect += RestartShifter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _shifter.OnStop -= TryIncrementCurrentPointIndex;
        CachedCharacter.OnTakeStatEffect -= RestartShifter;
    }

    private void TryIncrementCurrentPointIndex()
    {
        if (Vector3.Distance(transform.position, CurrentPoint.position) < 1f)
        {
            _currentPointIndex++;
        }
    }

    private void RestartShifter()
    {
        if (CachedCharacter.IsJumping == true)
        {
            return;
        }

        _shifter.Restart(this);
    }
}