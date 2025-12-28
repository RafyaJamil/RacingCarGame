using Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IAudio
    {
        void AddSound(AudioTrack sound);
        void PlaySound(string name);
        void Stop(string name);
        void StopAll();
        void SetVolume(string name, float volume);
    }
}
