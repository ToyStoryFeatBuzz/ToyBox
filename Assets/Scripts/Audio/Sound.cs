using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Sound
{
    [FormerlySerializedAs("_name")] public string Name;
    [FormerlySerializedAs("_clip")] public AudioClip Clip;
}
