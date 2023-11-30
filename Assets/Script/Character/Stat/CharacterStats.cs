using System;

[Serializable]
public struct CharacterStats
{
    public float Speed;
    public float JumpHeight;
    public float JumpSpeed;
    public float JumpLength;


    public CharacterStats(float speed, float jumpHeight, float jumpSpeed, float jumpLength)
    {
        Speed = speed;
        JumpHeight = jumpHeight;
        JumpSpeed = jumpSpeed;
        JumpLength = jumpLength;
    }

    public static CharacterStats operator +(CharacterStats left, CharacterStats right)
    {
        return new CharacterStats()
        {
            Speed = left.Speed + right.Speed,
            JumpHeight = left.JumpHeight + right.JumpHeight,
            JumpSpeed = left.JumpSpeed + right.JumpSpeed,
            JumpLength = left.JumpLength + right.JumpLength,
        };
    }

    public static CharacterStats operator -(CharacterStats left, CharacterStats right)
    {
        return new CharacterStats()
        {
            Speed = left.Speed - right.Speed,
            JumpHeight = left.JumpHeight - right.JumpHeight,
            JumpSpeed = left.JumpSpeed - right.JumpSpeed,
            JumpLength = left.JumpLength - right.JumpLength,
        };
    }

    public static CharacterStats operator *(CharacterStats left, CharacterStats right)
    {
        return new CharacterStats()
        {
            Speed = left.Speed * right.Speed,
            JumpHeight = left.JumpHeight * right.JumpHeight,
            JumpSpeed = left.JumpSpeed * right.JumpSpeed,
            JumpLength = left.JumpLength * right.JumpLength,
        };
    }

    public static CharacterStats operator /(CharacterStats left, CharacterStats right)
    {
        return new CharacterStats()
        {
            Speed = left.Speed / right.Speed,
            JumpHeight = left.JumpHeight / right.JumpHeight,
            JumpSpeed = left.JumpSpeed / right.JumpSpeed,
            JumpLength = left.JumpLength / right.JumpLength,
        };
    }
}