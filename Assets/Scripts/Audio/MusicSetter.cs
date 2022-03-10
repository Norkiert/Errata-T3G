using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using static Audio.AudioManager;

namespace Audio
{
    public class MusicSetter : MonoBehaviour
    {
        [SerializeField] private List<AudioClipSO> clips;
        [SerializeField, Min(0)] private float fadeTime = 1f;
        [SerializeField] private TrackType trackType = TrackType.Default;
        [SerializeField, ShowIf(nameof(IsCustomTrack)), Min(0)] private int track;

        private void Start()
        {
            if (clips.Count > 0)
            {
                if (IsCustomTrack())
                    AudioManager.PlayMusic(clips, fadeTime, track);
                else
                    AudioManager.PlayMusic(clips, fadeTime, (int)trackType);
            } 
            else
                Debug.LogWarning("Attempted to play music when list of musics is empty!");
        }

        private bool IsCustomTrack() => trackType == TrackType.Custom;
    }
}