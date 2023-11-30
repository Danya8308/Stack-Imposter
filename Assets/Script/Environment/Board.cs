using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Board : MonoBehaviour
{
    [SerializeField] private float _disappearTime = 4f;

    private Collider _collider;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Disappear()
    {
        void setActive(bool value)
        {
            var mesh = GetComponent<MeshRenderer>();

            mesh.enabled = value;
            _collider.enabled = value;
        }

        IEnumerator disappear()
        {
            setActive(false);

            yield return new WaitForSeconds(_disappearTime);

            setActive(true);
        }

        StartCoroutine(disappear());
    }
}