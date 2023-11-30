using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private string _idleName = "Idle";
    [SerializeField] private string _runName = "Run";

    private Character _character;


    private Animation Animation => GetComponentInChildren<Animation>();

    protected void Awake()
    {
        _character = GetComponent<Character>();

        PlayIdle();
    }

    protected void OnEnable()
    {
        Level.OnStart += PlayRun;
        _character.OnWin += PlayIdle;
    }

    protected void OnDisable()
    {
        Level.OnStart -= PlayRun;
        _character.OnWin -= PlayIdle;
    }

    private void PlayIdle()
    {
        Animation.Play(_idleName);
    }

    private void PlayRun()
    {
        Animation.Play(_runName);
    }
}