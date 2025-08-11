using UnityEngine;

public class UIWarpUtility : MonoBehaviour
{
    [SerializeField] private Color _warpScreenColor;

    public void Warp(string targetScene)
    {
        Time.timeScale = 1f;

        if (targetScene == "currLevel")
        {
            WarpManager.Instance.Warp(WarpManager.Instance.GetCurrentScene(), _warpScreenColor);
            return;
        }

        WarpManager.Instance.Warp(targetScene, _warpScreenColor);
    }
}
