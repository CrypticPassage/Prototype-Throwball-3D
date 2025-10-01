using Enums;
using Models;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource SFXSource;
        [SerializeField] private AudioClipVo[] audioClips;
        
        private void Start()
        {
            PlayMusicByType(EAudioType.MainMenu, true);
        }

        public void PlayMusicByType(EAudioType audioType, bool isMusicSource)
        {
            var source = isMusicSource ? musicSource : SFXSource;
            
            foreach (var audioClip in audioClips)
            {
                if (audioClip.AudioType == audioType)
                {
                    source.clip = audioClip.AudioClip;
                    source.Play();
                    
                    break;
                }
            }
        }
    }
}