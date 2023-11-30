using UnityEngine;

public class BotSkinChanger : CharacterSkinChanger
{
    protected void Awake()
    {
        SetBody(Random.Range(0, 4));
        SetBoard(Random.Range(0, 4));
    }
}