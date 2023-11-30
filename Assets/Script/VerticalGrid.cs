using UnityEngine;

public class VerticalGrid : MonoBehaviour
{
    [SerializeField] private float _spacing = 0.325f;


    public bool IsEmpty => Count < 1;

    public int Count => transform.childCount;

    public void Add(Transform itemPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Add(itemPrefab);
        }
    }

    public void Add(Transform itemPrefab)
    {
        var item = Instantiate(itemPrefab, transform);
        
        item.localPosition = item.up * _spacing * (transform.childCount - 1);
        item.localEulerAngles = Vector3.zero;

        if (item.TryGetComponent(out Collider collider) == true)
        {
            collider.enabled = false;
        }
    }

    public void RemoveLast()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
