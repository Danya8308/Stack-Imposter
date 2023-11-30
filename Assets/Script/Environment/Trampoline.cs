using System.Collections;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats = new CharacterStats(0f, 18f, -3.5f, 36f);


    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character) == false)
        {
            return;
        }

        StartCoroutine(JumpWithStats(character));
    }

    private IEnumerator JumpWithStats(Character character)
    {
        character.StopJump();

        var resetStats = false;

        character.TakeStatEffectWhile(_stats, () => resetStats == false);

        character.Jump();

        yield return new WaitWhile(() => character.IsJumping == true);

        resetStats = true;
    }
}