using UnityEngine;

public class ContactPortal : MonoBehaviour
{
    [SerializeField] private string _targetScene = "";
    [SerializeField] private Color _color = Color.white;

    private void OnTriggerEnter(Collider other)
    {
        WarpManager.Instance.Warp(_targetScene, _color);
    }
}
