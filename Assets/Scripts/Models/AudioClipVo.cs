using System;
using Enums;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class AudioClipVo
    {
        public EAudioType AudioType;
        public AudioClip AudioClip;
    }
}