using UnityEngine;
using UnityEngine.Assertions;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip secretsOfThothClip;
        [SerializeField] private AudioClip firstPharaohClip;

        private AudioSource _secretsOfThothSource;
        private AudioSource _firstPharaohSource;

        private void OnValidate()
        {
            Assert.IsNotNull(secretsOfThothClip);
            Assert.IsNotNull(firstPharaohClip);
        }

        private void Awake()
        {
            _secretsOfThothSource = gameObject.AddComponent<AudioSource>();
            _firstPharaohSource = gameObject.AddComponent<AudioSource>();

            _secretsOfThothSource.clip = secretsOfThothClip;
            _secretsOfThothSource.loop = true;
            _secretsOfThothSource.playOnAwake = true;

            _firstPharaohSource.clip = firstPharaohClip;
            _firstPharaohSource.loop = false;
            _firstPharaohSource.playOnAwake = false;
        }

        private void Start()
        {
            PlaySecretsOfThoth();
        }

        public void PlaySecretsOfThoth()
        {
            if (!_secretsOfThothSource.isPlaying)
            {
                _secretsOfThothSource.Play();
                _firstPharaohSource.Stop();
            }
        }

        public void PlayFirstPharaoh()
        {
            if (!_firstPharaohSource.isPlaying)
            {
                _firstPharaohSource.Play();
                _secretsOfThothSource.Stop();
                _firstPharaohSource.loop = true;
            }
        }
    }
}