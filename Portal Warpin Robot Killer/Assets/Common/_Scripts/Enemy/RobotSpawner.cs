using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _count;

    private void Start()
    {
        for (int i = 0; i < _count; i++)
        {
            Instantiate(_prefab, new(i, 0, 0), Quaternion.identity);
        }
    }
}