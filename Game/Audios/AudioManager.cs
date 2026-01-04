using Game.Core;
using Game.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Audios
{
    internal class AudioManager
    {
        private static Audio audio = new Audio();
        private static bool bgmPlaying = false;

        public static void Init()
        {
            audio.AddSound(new AudioTrack(
                "bgm", 
                @"Sounds\bgmusic.mp3",
                true   // LOOP
            ));

            audio.AddSound(new AudioTrack(
                "jump",
                @"Sounds\jump.wav",
                false
            ));

            audio.AddSound(new AudioTrack(
                "collision",
                @"Sounds\collision.wav",
                false
            ));
            audio.AddSound(new AudioTrack(
                "crash",
                @"Sounds\crash.wav",
                false
            ));

            audio.AddSound(new AudioTrack(
                "win",
                @"Sounds\win.wav",
                false
            ));

            audio.AddSound(new AudioTrack(
                "energyEater",
                @"Sounds\energyEater.wav",
                false
            ));
        }

        public static bool IsPlaying(string sound)
        {
            if (sound == "bgm") return bgmPlaying;
            return false;
        }

        public static void Play(string name) => audio.PlaySound(name); 
        public static void Stop(string name) => audio.Stop(name);

        public static void StopAll() => audio.StopAll();
        public static void SetVolume(string name, float volume) =>
            audio.SetVolume(name, volume);
    }
}
