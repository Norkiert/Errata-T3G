using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioSourceWrapper
    {
        public AudioSource Source { get; set; }
        private Queue<AudioSource> queue;

        public bool Valid
        {
            get
            {
                bool value = Source != null && queue != null && !queue.Contains(Source);
                if (!value)
                {
                    Source = null;
                    queue = null;
                    Debug.LogWarning("Tried to use invalid AudioSourceWrapper");
                }
                return value;
            }
        }

        public float Volume
        {
            get
            {
                return Valid ? Source.volume : float.NaN;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                if (value < AudioClipSO.VolumeMin)
                {
                    Source.volume = AudioClipSO.VolumeMin;
                }
                else if(value > AudioClipSO.VolumeMax)
                {
                    Source.volume = AudioClipSO.VolumeMax;
                }
                else
                {
                    Source.volume = value;
                }
            }
        }
        public float Pitch
        {
            get
            {
                return Valid ? Source.pitch : float.NaN;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                if (value < AudioClipSO.PitchMin)
                {
                    Source.pitch = AudioClipSO.PitchMin;
                }
                else if (value > AudioClipSO.PitchMax)
                {
                    Source.pitch = AudioClipSO.PitchMax;
                }
                else
                {
                    Source.pitch = value;
                }
            }
        }
        public float StereoPan
        {
            get
            {
                return Valid ? Source.panStereo : float.NaN;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                if (value < AudioClipSO.StereoPanMin)
                {
                    Source.panStereo = AudioClipSO.StereoPanMin;
                }
                else if (value > AudioClipSO.StereoPanMax)
                {
                    Source.panStereo = AudioClipSO.StereoPanMax;
                }
                else
                {
                    Source.panStereo = value;
                }
            }
        }
        public float SpiralBlend
        {
            get
            {
                return Valid ? Source.volume : float.NaN;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                if (value < AudioClipSO.SpiralBlendMin)
                {
                    Source.spatialBlend = AudioClipSO.SpiralBlendMin;
                }
                else if (value > AudioClipSO.SpiralBlendMax)
                {
                    Source.spatialBlend = AudioClipSO.SpiralBlendMax;
                }
                else
                {
                    Source.spatialBlend = value;
                }
            }
        }
        public float ReverbZoneMix
        {
            get
            {
                return Valid ? Source.reverbZoneMix : float.NaN;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                if (value < AudioClipSO.ReverbZoneMixMin)
                {
                    Source.reverbZoneMix = AudioClipSO.ReverbZoneMixMin;
                }
                else if (value > AudioClipSO.ReverbZoneMixMax)
                {
                    Source.reverbZoneMix = AudioClipSO.ReverbZoneMixMax;
                }
                else
                {
                    Source.reverbZoneMix = value;
                }
            }
        }
        public bool Loop
        {
            get
            {
                return Valid && Source.loop;
            }
            set
            {
                if (!Valid)
                {
                    return;
                }
                Source.loop = value;
            }
        }
        public Vector3 Position
        {
            get
            {
                if (!Valid)
                {
                    return Vector3.positiveInfinity;
                }
                else
                {
                    return Source.transform.position;
                }
            }
            set
            {
                if (!Valid)
                {
                    return;
                }

                Source.transform.position = value;
            }
        }

        public bool Paused { get; protected set; } = false;
        public bool Stopped { get; protected set; } = false;
        public bool Playing { get; protected set; } = false;
        /// <summary> Active means that wrapper holds AudioSource even if not playing </summary>
        public bool Active { get; set; } = false;

        public AudioSourceWrapper(AudioSource source, Queue<AudioSource> queue)
        {
            Source = source;
            this.queue = queue;

            if (Source)
            {
                Playing = true;
            }
        }

        public void Pause()
        {
            Source.Pause();

            Paused = true;
            Playing = false;
        }
        public void UnPause()
        {
            Source.UnPause();

            Paused = false;
            Playing = true;
        }
        public void Stop()
        {
            Source.Stop();

            Stopped = false;
            Paused = false;
            Playing = false;
        }
        public void Play()
        {
            Source.Play();

            Playing = true;
            Paused = false;
            Stopped = false;
        }
        public void Deactivate()
        {
            if (Valid) Stop();
            Active = false;
        }
    }
}
