using System;
using System.IO;
using System.Linq;

namespace Game
{
    internal static class FileManager
    {
        private static string filePath = "playerdata.txt";

        // 🔍 Check player exists or not
        public static bool PlayerExists(string name)
        {
            if (!File.Exists(filePath)) return false;

            return File.ReadAllLines(filePath)
                       .Any(line => line.StartsWith(name + ","));
        }

        // 📥 Get last completed level
        public static int GetPlayerLevel(string name)
        {
            if (!File.Exists(filePath)) return 0;

            var line = File.ReadAllLines(filePath)
                           .FirstOrDefault(l => l.StartsWith(name + ","));

            if (line == null) return 0;

            string[] parts = line.Split(',');
            return int.Parse(parts[1]);
        }

        // 💾 Save / Update player
        public static void SavePlayer(string name, int levelCompleted, int score)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            var lines = File.ReadAllLines(filePath).ToList();
            int index = lines.FindIndex(l => l.StartsWith(name + ","));

            if (index >= 0)
            {
                // update only if level is higher
                string[] parts = lines[index].Split(',');
                int oldLevel = int.Parse(parts[1]);

                if (levelCompleted > oldLevel)
                {
                    lines[index] = $"{name},{levelCompleted},{score}";
                }
            }
            else
            {
                // new player
                lines.Add($"{name},{levelCompleted},{score}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
