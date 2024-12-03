using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Watermelon
{
    [RequireSetting("Vibration", PrefsSettings.FieldType.Bool)]
    [RequireSetting("Volume", PrefsSettings.FieldType.Float)]
    public class AudioController
    {
        private static AudioController instance;

        private FieldInfo[] fields;
        private const int AUDIO_SOURCES_AMOUNT = 4;

        private GameObject targetGameObject;

        private List<AudioSource> audioSources = new List<AudioSource>();

        private List<AudioSource> activeSoundSources = new List<AudioSource>();
        private List<AudioSource> activeMusicSources = new List<AudioSource>();

        private List<AudioSource> customSources = new List<AudioSource>();
        private List<AudioCaseCustom> activeCustomSourcesCases = new List<AudioCaseCustom>();

        private static bool vibrationState;
        private static float volume;

        private static AudioClip[] musicAudioClips;
        public static AudioClip[] MusicAudioClips => musicAudioClips;

        private static Sounds sounds;
        public static Sounds Sounds => sounds;

        private static Music music;
        public static Music Music => music;

        private static Vibrations vibrations;
        public static Vibrations Vibrations => vibrations;

        public static OnVolumeChangedCallback OnVolumeChanged;
        public static OnVibrationChangedCallback OnVibrationChanged;

        private static AudioListener audioListener;
        public static AudioListener AudioListener => audioListener;

        public void Init(AudioSettings settings, GameObject targetGameObject)
        {
            if(instance != null)
            {
                Debug.Log("[Audio Controller]: Module already exists!");

                return;
            }

            if (settings == null)
            {
                Debug.LogError("[AudioController]: Audio Settings is NULL! Please assign audio settings scriptable on Audio Controller script.");

                return;
            }

            this.targetGameObject = targetGameObject;

            instance = this;
            fields = typeof(Music).GetFields();
            musicAudioClips = new AudioClip[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                musicAudioClips[i] = fields[i].GetValue(settings.Music) as AudioClip;
            }

            music = settings.Music;
            sounds = settings.Sounds;
            vibrations = settings.Vibrations;

            //Create audio source objects
            audioSources.Clear();
            for (int i = 0; i < AUDIO_SOURCES_AMOUNT; i++)
            {
                audioSources.Add(CreateAudioSourceObject(false));
            }

            // Load default states
            vibrationState = PrefsSettings.GetBool(PrefsSettings.Key.Vibration);
            volume = PrefsSettings.GetFloat(PrefsSettings.Key.Volume);
        }

        public static void CreateAudioListener()
        {
            if (audioListener != null)
                return;

            // Create game object for listener
            GameObject listenerObject = new GameObject("[AUDIO LISTENER]");
            listenerObject.transform.position = Vector3.zero;

            // Mark as non-destroyable
            GameObject.DontDestroyOnLoad(listenerObject);

            // Add listener component to created object
            audioListener = listenerObject.AddComponent<AudioListener>();
        }

        public static bool IsVibrationModuleEnabled()
        {
            return PrefsSettings.GetBool(PrefsSettings.Key.Vibration);
        }

        public static bool IsAudioModuleEnabled()
        {
            return (!((PrefsSettings.GetFloat(PrefsSettings.Key.Volume) - 0.00005f) < 0)); 
        }

        public static void PlayRandomMusic()
        {
            if(!musicAudioClips.IsNullOrEmpty())
                PlayMusic(musicAudioClips.GetRandomItem());
        }

        /// <summary>
        /// Stop all active streams
        /// </summary>
        public static void ReleaseStreams()
        {
            ReleaseMusic();
            ReleaseSounds();
            ReleaseCustomStreams();
        }

        /// <summary>
        /// Releasing all active music.
        /// </summary>
        public static void ReleaseMusic()
        {
            int activeMusicCount = instance.activeMusicSources.Count - 1;
            for (int i = activeMusicCount; i >= 0; i--)
            {
                instance.activeMusicSources[i].Stop();
                instance.activeMusicSources[i].clip = null;
                instance.activeMusicSources.RemoveAt(i);
            }
        }

        /// <summary>
        /// Releasing all active sounds.
        /// </summary>
        public static void ReleaseSounds()
        {
            int activeStreamsCount = instance.activeSoundSources.Count - 1;
            for (int i = activeStreamsCount; i >= 0; i--)
            {
                instance.activeSoundSources[i].Stop();
                instance.activeSoundSources[i].clip = null;
                instance.activeSoundSources.RemoveAt(i);
            }
        }

        /// <summary>
        /// Releasing all active custom sources.
        /// </summary>
        public static void ReleaseCustomStreams()
        {
            int activeStreamsCount = instance.activeCustomSourcesCases.Count - 1;
            for (int i = activeStreamsCount; i >= 0; i--)
            {
                if (instance.activeCustomSourcesCases[i].autoRelease)
                {
                    AudioSource source = instance.activeCustomSourcesCases[i].source;
                    instance.activeCustomSourcesCases[i].source.Stop();
                    instance.activeCustomSourcesCases[i].source.clip = null;
                    instance.activeCustomSourcesCases.RemoveAt(i);
                    instance.customSources.Add(source);
                }
            }
        }

        public static void StopStream(AudioCase audioCase, float fadeTime = 0)
        {
            if (audioCase.type == AudioType.Sound)
            {
                instance.StopSound(audioCase.source, fadeTime);
            }
            else
            {
                instance.StopMusic(audioCase.source, fadeTime);
            }
        }

        public static void StopStream(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            ReleaseCustomSource(audioCase, fadeTime);
        }

        private void StopSound(AudioSource source, float fadeTime = 0)
        {
            int streamID = activeSoundSources.FindIndex(x => x == source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    activeSoundSources[streamID].Stop();
                    activeSoundSources[streamID].clip = null;
                    activeSoundSources.RemoveAt(streamID);
                }
                else
                {
                    activeSoundSources[streamID].DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeSoundSources.Remove(source);
                        source.Stop();
                    });
                }
            }
        }

        private void StopMusic(AudioSource source, float fadeTime = 0)
        {
            int streamID = activeMusicSources.FindIndex(x => x == source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    activeMusicSources[streamID].Stop();
                    activeMusicSources[streamID].clip = null;
                    activeMusicSources.RemoveAt(streamID);
                }
                else
                {
                    activeMusicSources[streamID].DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeMusicSources.Remove(source);
                        source.Stop();
                    });
                }
            }
        }

        private static void AddMusic(AudioSource source)
        {
            if (!instance.activeMusicSources.Contains(source))
            {
                instance.activeMusicSources.Add(source);
            }
        }

        private static void AddSound(AudioSource source)
        {
            if (!instance.activeSoundSources.Contains(source))
            {
                instance.activeSoundSources.Add(source);
            }
        }

        public static void PlayMusic(AudioClip clip, float volumePercentage = 1.0f)
        {
            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            AudioSource source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.Play();

            AddMusic(source);
        }

        public static AudioCase PlaySmartMusic(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            AudioSource source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            AudioCase audioCase = new AudioCase(clip, source, AudioType.Music);

            audioCase.Play();

            AddMusic(source);

            return audioCase;
        }


        public static void PlaySound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            AudioSource source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Sound);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;
            source.Play();

            AddSound(source);
        }

        public static AudioCase PlaySmartSound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            AudioSource source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Sound);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            AudioCase audioCase = new AudioCase(clip, source, AudioType.Sound);
            audioCase.Play();

            AddSound(source);

            return audioCase;
        }

        public static AudioCaseCustom GetCustomSource(bool autoRelease, AudioType audioType)
        {
            AudioSource source = null;

            if (!instance.customSources.IsNullOrEmpty())
            {
                source = instance.customSources[0];
                instance.customSources.RemoveAt(0);
            }
            else
            {
                source = instance.CreateAudioSourceObject(true);
            }

            SetSourceDefaultSettings(source, audioType);

            AudioCaseCustom audioCase = new AudioCaseCustom(null, source, audioType, autoRelease);

            instance.activeCustomSourcesCases.Add(audioCase);

            return audioCase;
        }

        public static void ReleaseCustomSource(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            int streamID = instance.activeCustomSourcesCases.FindIndex(x => x.source == audioCase.source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    instance.activeCustomSourcesCases[streamID].source.Stop();
                    instance.activeCustomSourcesCases[streamID].source.clip = null;
                    instance.activeCustomSourcesCases.RemoveAt(streamID);
                    instance.customSources.Add(audioCase.source);
                }
                else
                {
                    instance.activeCustomSourcesCases[streamID].source.DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        instance.activeCustomSourcesCases.Remove(audioCase);
                        audioCase.source.Stop();
                        instance.customSources.Add(audioCase.source);
                    });
                }
            }
        }

        private AudioSource GetAudioSource()
        {
            int sourcesAmount = audioSources.Count;
            for (int i = 0; i < sourcesAmount; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    return audioSources[i];
                }
            }

            AudioSource createdSource = CreateAudioSourceObject(false);
            audioSources.Add(createdSource);

            return createdSource;
        }

        private AudioSource CreateAudioSourceObject(bool isCustom)
        {
            AudioSource audioSource = targetGameObject.AddComponent<AudioSource>();
            SetSourceDefaultSettings(audioSource);

            return audioSource;
        }

        private void SetVolumeForAudioSources(float volume)
        {
            SetSoundsVolume(volume);
            SetMusicVolume(volume);

            for (int i = 0; i < activeCustomSourcesCases.Count; i++)
            {
                activeCustomSourcesCases[i].source.volume = volume;
            }
        }

        public static void SetSoundsVolume(float newVolume)
        {
            for (int i = 0; i < instance.activeSoundSources.Count; i++)
            {
                instance.activeSoundSources[i].volume = newVolume;
            }
        }

        public static void SetMusicVolume(float newVolume)
        {
            for (int i = 0; i < instance.activeMusicSources.Count; i++)
            {
                instance.activeMusicSources[i].volume = newVolume;
            }
        }

        public static void SetVolume(float volume)
        {
            AudioController.volume = volume;

            PrefsSettings.SetFloat(PrefsSettings.Key.Volume, volume);

            instance.SetVolumeForAudioSources(volume);

            OnVolumeChanged?.Invoke(volume);
        }

       

        public static float GetVolume()
        {
            return volume;
        }

        public static bool IsVibrationEnabled()
        {
            return vibrationState;
        }

        public static void SetVibrationState(bool vibrationState)
        {
            AudioController.vibrationState = vibrationState;

            PrefsSettings.SetBool(PrefsSettings.Key.Vibration, vibrationState);

            OnVibrationChanged?.Invoke(vibrationState);
        }

        public static void SetSourceDefaultSettings(AudioSource source, AudioType type = AudioType.Sound)
        {
            float volume = PrefsSettings.GetFloat(PrefsSettings.Key.Volume);

            if (type == AudioType.Sound)
            {
                source.loop = false;
            }
            else if (type == AudioType.Music)
            {
                source.loop = true;
            }

            source.clip = null;

            source.volume = volume;
            source.pitch = 1.0f;
            source.spatialBlend = 0; // 2D Sound
            source.mute = false;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = null;
        }

        public enum AudioType
        {
            Music = 0,
            Sound = 1
        }

        public delegate void OnVolumeChangedCallback(float volume);
        public delegate void OnVibrationChangedCallback(bool state);
    }
}

// -----------------
// Audio Controller v 0.3.4
// -----------------

// Changelog
// v 0.3.3
// • Method for separate music and sound volume override
// v 0.3.2
// • Added audio listener creation method
// v 0.3.2
// • Added volume float
// • AudioSettings variable removed (now sounds, music and vibrations can be reached directly)
// v 0.3.1
// • Added OnVolumeChanged callback
// • Renamed AudioSettings to Settings
// v 0.3
// • Added IsAudioModuleEnabled method
// • Added IsVibrationModuleEnabled method
// • Removed VibrationToggleButton class
// v 0.2
// • Removed MODULE_VIBRATION
// v 0.1
// • Added basic version
// • Added support of new initialization
// • Music and Sound volume is combined