using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    //public string levelID;
    public AudioResource introAudio, winAudio;
    public string introCaption, winCaption;
}