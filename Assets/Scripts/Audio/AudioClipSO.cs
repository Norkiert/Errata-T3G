using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Audio
{
    [CreateAssetMenu(fileName = "newAudioClipSO", menuName = "Audio/AudioClipSO")]
    public class AudioClipSO : ScriptableObject
    {
        private enum Type { Single, Multiple }

        [SerializeField] private Type type = Type.Single;
        [SerializeField, HideIf(nameof(HasMultipleClips))] private AudioClip clip;
        [SerializeField, ShowIf(nameof(HasMultipleClips))] private List<AudioClip> clips = new List<AudioClip>();

        public const float VolumeMin = 0;
        public const float VolumeMax = 1;
        [field: SerializeField, Range(VolumeMin, VolumeMax)] public float Volume { get; private set; } = 0.5f;

        public const float PitchMin = -3;
        public const float PitchMax = 3;
        [field: SerializeField, Range(PitchMin, PitchMax)] public float Pitch { get; private set; } = 1f;
        public const float StereoPanMin = -1;
        public const float StereoPanMax = 1;
        [field: SerializeField, Range(StereoPanMin, StereoPanMax)] public float StereoPan { get; private set; } = 0f;
        public const float SpiralBlendMin = 0;
        public const float SpiralBlendMax = 1;
        [field: SerializeField, Range(SpiralBlendMin, SpiralBlendMax)] public float SpiralBlend { get; private set; } = 0f;
        public const float ReverbZoneMixMin = 0;
        public const float ReverbZoneMixMax = 1.1f;
        [field: SerializeField, Range(ReverbZoneMixMin, ReverbZoneMixMax)] public float ReverbZoneMix { get; private set; } = 1f;


        public void ApplySettingsForAudioSource(AudioSource source, float volumeMultiplier = 1)
        {
            source.clip = Clip;

            source.volume = Volume * volumeMultiplier;
            source.pitch = Pitch;
            source.panStereo = StereoPan;
            source.spatialBlend = SpiralBlend;
            source.reverbZoneMix = ReverbZoneMix;
        }

        public AudioClip Clip => type == Type.Single ? clip : clips[Random.Range(0, clips.Count)];

        public bool HasMultipleClips() => type == Type.Multiple;
    }
}
