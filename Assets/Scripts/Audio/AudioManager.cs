using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (instance != null)
                return;

            GameObject newAudioManager = new GameObject();
            newAudioManager.AddComponent<AudioManager>();
            newAudioManager.name = nameof(AudioManager);
            DontDestroyOnLoad(newAudioManager);
        }

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
        }

        private static AudioManager instance;


        private static readonly Queue<AudioSource> unusedAudioSources = new Queue<AudioSource>();


        private const string keyGeneralVolume = "GeneralVolume";
        public static float GeneralVolume => PlayerPrefs.GetFloat(keyGeneralVolume, 1f);
        public static void SetGeneralVolume(float volume)
        {
            PlayerPrefs.SetFloat(keyGeneralVolume, volume);
            UpdateTrackVolume();
        }


        #region -Music-

        private const string keyMusicVolume = "MusicVolume";
        public static float MusicVolume => PlayerPrefs.GetFloat(keyMusicVolume, 0.5f);
        public static void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(keyMusicVolume, volume);

            UpdateTrackVolume();
        }
        private static void UpdateTrackVolume()
        {
            foreach (Track track in tracks.Values)
                track.UpdateVolume(MusicVolume * GeneralVolume);
        }


        public enum TrackType { Default = 0, Additional = -1, Custom = 2 }
        private static readonly Dictionary<int, Track> tracks = new Dictionary<int, Track>();
        

        public static void PlayMusic(AudioClipSO clip, float fadeTime, int trackID = 0)
        {
            if (clip == null)
            {
                Debug.LogWarning($"Attempted to play music on track {trackID} with no clip!");
                return;
            }

            PlayMusic(new List<AudioClipSO> { clip }, fadeTime, trackID);
        }
        public static void PlayMusic(List<AudioClipSO> clips, float fadeTime, int trackID = 0)
        {
            if (clips == null || clips.Count == 0)
            {
                Debug.LogWarning($"Attempted to play music on track {trackID} with no clip!");
                return;
            }

            if (trackID == -1)
                trackID = GetFreeAditionalMusicTrackID();

            if (!tracks.TryGetValue(trackID, out Track track))
            {
                track = new Track(trackID);
                tracks.Add(trackID, track);
            }

            track.PlayMusic(clips, fadeTime);
        }

        public static void StopMusic(int trackID = 0)
        {
            if (tracks.TryGetValue(trackID, out Track track))
            {
                track.Desactive();
                tracks.Remove(trackID);
            }
        }
        public static void StopAllMusic()
        {
            var ids = tracks.Keys.ToArray();
            foreach (int id in ids)
                StopMusic(id);
        }

        private static int GetFreeAditionalMusicTrackID()
        {
            int id = -1;

            for (int i = 0; i < tracks.Values.Count; i++)
                if (tracks.ContainsKey(id))
                    id--;
                else
                    return id;

            return id;
        }

        #endregion

        #region -SFX-

        private const string keySFXVolume = "SFXVolume";
        public static float SFXVolume => PlayerPrefs.GetFloat(keySFXVolume, 0.5f);
        public static void SetSFXVolume(float volume) => PlayerPrefs.SetFloat(keySFXVolume, volume);


        public static AudioSourceWrapper PlaySFX(AudioClipSO sfx) => SetAndPlaySFX(sfx, Vector3.zero, null);
        public static AudioSourceWrapper PlaySFX(AudioClipSO sfx, Vector3 position) => SetAndPlaySFX(sfx, position, null);
        public static AudioSourceWrapper PlaySFX(AudioClipSO sfx, Transform parent) => SetAndPlaySFX(sfx, Vector3.zero, parent);
        public static AudioSourceWrapper PlaySFX(AudioClipSO sxf, Transform parent, Vector3 offset) => SetAndPlaySFX(sxf, offset, parent);
        private static AudioSourceWrapper SetAndPlaySFX(AudioClipSO sfx, Vector3 offset, Transform parent)
        {
            if (sfx.Clip == null)
            {
                Debug.LogWarning($"{sfx.name} sfx has no audio clip");
                return null;
            }

            AudioSource source = GetFreeAudioSource();
            if (source == null)
            {
                Debug.LogWarning("Invalid free aadio source");
                return null;
            }

            if (parent == null)
            {
                source.transform.parent = instance.transform;
                source.transform.position = offset;
            }
            else
            {
                source.transform.parent = parent;
                source.transform.localPosition = offset;
            }

            sfx.ApplySettingsForAudioSource(source, GeneralVolume * SFXVolume);
            source.loop = false;
            source.gameObject.name = $"SFX-{source.clip.name}";
            source.gameObject.SetActive(true);
            source.Play();

            var wrapper = new AudioSourceWrapper(source, unusedAudioSources);

            instance.StartCoroutine(DeactivateSource(new System.WeakReference<AudioSourceWrapper>(wrapper), source));

            return wrapper;
            //instance.StartCoroutine(DesactiveSource(source, source.clip.length));
        }


        #endregion


        private static AudioSource GetFreeAudioSource()
        {
            if (instance == null)
            {
                Debug.LogWarning("Tried play sound on unload scene");
                return null;
            }

            if (unusedAudioSources.Count > 0)
            {
                AudioSource source = unusedAudioSources.Dequeue();
                return source;
            }

            AudioSource newSource = new GameObject().AddComponent<AudioSource>();
            newSource.transform.parent = instance.transform;
            newSource.playOnAwake = false;

            return newSource;
        }

        protected static IEnumerator DeactivateSource(System.WeakReference<AudioSourceWrapper> reference, AudioSource source)
        {
            bool deactivatedLoop = false;
            for(; ; )
            {
                if(reference.TryGetTarget(out AudioSourceWrapper wrapper))
                {
                    if (wrapper.Valid && !wrapper.Playing && wrapper.Stopped && !wrapper.Active || !source.isPlaying && source.time == 0)
                    {
                        wrapper.Source = null; 
                        DesactiveSource(wrapper.Source);
                        yield break;
                    }
                    else if (!wrapper.Valid && !unusedAudioSources.Contains(source))
                    {
                        if (!deactivatedLoop)
                        {
                            source.loop = false;
                            deactivatedLoop = true;
                        }
                        if (!source.isPlaying && source.time == 0)
                        {
                            DesactiveSource(source);
                            yield break;
                        }
                    }
                }
                else
                {
                    if (!deactivatedLoop)
                    {
                        source.loop = false;
                        deactivatedLoop = true;
                    }
                    if(!source.isPlaying && source.time == 0)
                    {
                        DesactiveSource(source);
                        yield break;
                    }
                }

                yield return null;
            }
        }

        private static IEnumerator DesactiveSource(AudioSource source, float time)
        {
            yield return new WaitForSeconds(time);

            if (source.gameObject != null)
                DesactiveSource(source);
        }
        private static void DesactiveSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);

            unusedAudioSources.Enqueue(source);
        }

        private class Track
        {
            private readonly int id;

            private float defaultClipVolume = 1f;
            private AudioSource musicSource;

            private IEnumerator clipChanger;
            private IEnumerator multipleMusicSelector;

            public Track(int id)
            {
                this.id = id;
            }

            public void PlayMusic(AudioClipSO clip, float fadeTime)
            {
                if (clip == null)
                {
                    Debug.LogWarning($"Attempted to play music on track {id} with no clip!");
                    return;
                }

                if (musicSource == null)
                {
                    musicSource = GetFreeAudioSource();
                    musicSource.loop = true;
                }
                defaultClipVolume = clip.Volume;

                if (clipChanger != null)
                    instance.StopCoroutine(clipChanger);
                clipChanger = ChangeClipOnTrack(clip, fadeTime);
                instance.StartCoroutine(clipChanger);
            }
            public void PlayMusic(List<AudioClipSO> clips, float fadeTime)
            {
                if (clips.Count == 1)
                {
                    PlayMusic(clips[0], fadeTime);
                    return;
                }

                if (multipleMusicSelector != null)
                    instance.StopCoroutine(multipleMusicSelector);
                multipleMusicSelector = SelectMusicFromMultiple(clips, fadeTime);
                instance.StartCoroutine(multipleMusicSelector);
            }

            public void UpdateVolume(float volume)
            {
                if (musicSource)
                    musicSource.volume = volume * defaultClipVolume;
            }

            public void Desactive()
            {
                if (clipChanger != null)
                    instance.StopCoroutine(clipChanger);

                if (multipleMusicSelector != null)
                    instance.StopCoroutine(multipleMusicSelector);

                DesactiveSource(musicSource);
            }

            private IEnumerator SelectMusicFromMultiple(List<AudioClipSO> clips, float fadeTime)
            {
                while (true)
                {
                    AudioClipSO clipToPlay = clips[Random.Range(0, clips.Count)];
                    PlayMusic(clipToPlay, fadeTime);

                    yield return new WaitForSeconds(clipToPlay.Clip.length + fadeTime * 2);
                }
            }

            private IEnumerator ChangeClipOnTrack(AudioClipSO clip, float fadeTime)
            {
                if (musicSource.clip != null)
                {
                    while (musicSource.volume > 0)
                    {
                        musicSource.volume -= Time.deltaTime / fadeTime;
                        yield return null;
                    }
                    musicSource.Stop();
                }
                
                clip.ApplySettingsForAudioSource(musicSource, 0);
                musicSource.name = $"Music-{clip.name}";
                musicSource.Play();

                float targetVolume = MusicVolume * GeneralVolume * defaultClipVolume;
                while (musicSource.volume >= targetVolume)
                {
                    musicSource.volume += Time.deltaTime / fadeTime;
                    yield return null;
                }
                musicSource.volume = targetVolume;
                clipChanger = null;
            }
        }
    }
}
