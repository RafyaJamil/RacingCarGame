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
        //sound data
        private Dictionary<string, AudioTrack> sounds = new Dictionary<string, AudioTrack>();
        //playing devices
        private Dictionary<string, WaveOutEvent> outputs = new();
        // File readers
        private Dictionary<string, AudioFileReader> readers = new();

        public void AddSound(AudioTrack sound)
        {
            if (!sounds.ContainsKey(sound.Name))
            {
                sounds.Add(sound.Name, sound);
            }
        }
        public void PlaySound(string name)
        {
            try
            {
                if (!sounds.ContainsKey(name))
                    return;

                AudioTrack sound = sounds[name];

                string fullPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    sound.FilePath
                );

                if (!File.Exists(fullPath))
                    return; // silently ignore

                AudioFileReader reader = new AudioFileReader(fullPath);
                reader.Volume = sound.Volume;

                WaveOutEvent output = new WaveOutEvent();
                output.Init(reader);

                if (sound.Loop)
                {
                    output.PlaybackStopped += (s, e) =>
                    {
                        reader.Position = 0;
                        output.Play();
                    };
                }

                readers[name] = reader;
                outputs[name] = output;

                output.Play();
            }
            catch
            {
                // Game should NEVER crash because of audio
            }
        }

        public void Stop(string name)
        {
            if (!outputs.ContainsKey(name))
                return;

            outputs[name].Stop();
            outputs[name].Dispose();
            readers[name].Dispose();

            outputs.Remove(name);
            readers.Remove(name);
        }

        public void StopAll()
        {
            foreach (var name in outputs.Keys.ToList())
            {
                Stop(name);
            }
        }
        public void SetVolume(string name, float volume)
        {
            if (readers.ContainsKey(name))
            {
                readers[name].Volume = volume;
            }
        }
    }
}
