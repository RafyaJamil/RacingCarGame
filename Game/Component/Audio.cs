using System.IO;
using NAudio.Wave;
using Game.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Core;

namespace Game.Component
{
    public class Audio : IAudio
    {

        private Dictionary<string, AudioTrack> sounds = new Dictionary<string, AudioTrack>();
        private Dictionary<string, List<WaveOutEvent>> outputs = new();
        private Dictionary<string, List<AudioFileReader>> readers = new();

        public void AddSound(AudioTrack sound)
        {
            if (!sounds.ContainsKey(sound.Name))
                sounds.Add(sound.Name, sound);
        }

        public void PlaySound(string name, bool allowMultiple = false)
        {
            if (!sounds.ContainsKey(name)) return;

            if (!allowMultiple && outputs.ContainsKey(name) && outputs[name].Count > 0)
                return;

            try
            {
                AudioTrack sound = sounds[name];
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sound.FilePath);
                if (!File.Exists(fullPath)) return;

                AudioFileReader reader = new AudioFileReader(fullPath) { Volume = sound.Volume };
                WaveOutEvent output = new WaveOutEvent();
                output.Init(reader);
                output.Play();

                
                output.PlaybackStopped += (s, e) =>
                {
                    output.Dispose();
                    reader.Dispose();
                    if (outputs.ContainsKey(name)) outputs[name].Remove(output);
                    if (readers.ContainsKey(name)) readers[name].Remove(reader);
                };

                if (!outputs.ContainsKey(name)) outputs[name] = new List<WaveOutEvent>();
                if (!readers.ContainsKey(name)) readers[name] = new List<AudioFileReader>();

                outputs[name].Add(output);
                readers[name].Add(reader);
            }
            catch
            {
                
            }
        }

        public void Stop(string name)
        {
            if (!outputs.ContainsKey(name)) return;

            foreach (var output in outputs[name])
            {
                output.Stop();
                output.Dispose();
            }
            foreach (var reader in readers[name])
            {
                reader.Dispose();
            }
            outputs[name].Clear();
            readers[name].Clear();
        }

        public void StopAll()
        {
            foreach (var name in outputs.Keys)
                Stop(name);
        }

        public void SetVolume(string name, float volume)
        {
            if (readers.ContainsKey(name))
            {
                foreach (var reader in readers[name])
                    reader.Volume = volume;
            }
        }
    }
}
