using System.Collections;
using UnityEngine;
using AudioType = Watermelon.AudioController.AudioType;

namespace Watermelon
{
    [System.Serializable]
    public class AudioCase
    {
        public AudioSource source;

        public AudioType type;

        public AudioCallback onAudioEnded;
        private Coroutine endCoroutine;

        public AudioCase(AudioClip clip, AudioSource source, AudioType type, AudioCallback callback = null)
        {
            this.source = source;
            this.type = type;

            this.source.clip = clip;
        }

        public AudioCase OnComplete(AudioCallback callback)
        {
            onAudioEnded = callback;

            endCoroutine = Tween.InvokeCoroutine(OnAudioEndCoroutine(source.clip.length));

            return this;
        }

        public virtual void Play()
        {
            source.Play();
        }

        public void Stop()
        {
            source.Stop();

            if (endCoroutine != null)
                Tween.StopCustomCoroutine(endCoroutine);
        }

        public void FadeOut(float value, float time, bool stop = false)
        {
            TweenCase tweenCase = source.DOVolume(value, time);

            if (stop)
            {
                tweenCase.OnComplete(delegate
                {
                    source.Stop();
                });
            }
        }

        public void FadeIn(float value, float time)
        {
            source.DOVolume(value, time);
        }

        private IEnumerator OnAudioEndCoroutine(float clipDuration)
        {
            yield return new WaitForSeconds(clipDuration);

            onAudioEnded.Invoke();
        }

        public delegate void AudioCallback();
    }
}

// -----------------
// Audio Controller v 0.3.3
// -----------------