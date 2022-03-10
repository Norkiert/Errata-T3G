using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "newAudioClipSO", menuName = "Audio/AudioClipSO")]
    public class AudioClipSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float Volume { get; private set; } = 0.5f;

        [field: SerializeField, Range(-3f, 3f)] public float Pitch { get; private set; } = 1f;
        [field: SerializeField, Range(-1f, 1f)] public float StereoPan { get; private set; } = 0f;
        [field: SerializeField, Range(0f, 1f)] public float SpiralBlend { get; private set; } = 0f;
        [field: SerializeField, Range(0f, 1.1f)] public float ReverbZoneMix { get; private set; } = 1f;


        public void ApplySettingsForAudioSource(AudioSource source, float volumeMultiplier = 1)
        {
            source.clip = Clip;
            source.volume = Volume * volumeMultiplier;

            source.pitch = Pitch;
            source.panStereo = StereoPan;
            source.spatialBlend = SpiralBlend;
            source.reverbZoneMix = ReverbZoneMix;
        }
    }
}
