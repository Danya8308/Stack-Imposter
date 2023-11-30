using System;
using UnityEngine;

public class UserLogic : CharacterLogic
{
    public event Action<int> OnChangeCoins;


    public int CollectedCoins { get; private set; }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CachedCharacter.Move(Vector3.forward);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        CachedCharacter.OnTakeBoard += AddCoins;
        CachedCharacter.OnWin += SaveCoins;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        CachedCharacter.OnTakeBoard -= AddCoins;
        CachedCharacter.OnWin -= SaveCoins;
    }

    protected override void CollisionWithCharacter(Character collision)
    {
        if (collision.IsWinner == true)
        {
            return;
        }

        CachedCharacter.TakeBoards(collision.BoardCount);
        collision.Die();

        string name;

        if (collision.TryGetComponent(out HasNickname getter))
        {
            name = getter.Get();
        }
        else
        {
            name = NicknameCreator.GetRandom();
        }

        SystemMessenger.Send("You killed:\n" + name);
    }

    private void SaveCoins()
    {
        if (Finish.IsWin == false)
        {
            return;
        }

        int coins = Profile.IntegerValues.GetValue(IntegerSaveType.Cash);

        Profile.IntegerValues.Save(IntegerSaveType.Cash, coins + CollectedCoins * Finish.CoinsFactor);
    }

    private void AddCoins(int value)
    {
        if (value < 1)
        {
            return;
        }

        CollectedCoins += value;
        OnChangeCoins?.Invoke(CollectedCoins);
    }
}