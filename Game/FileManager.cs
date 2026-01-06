using System;
using System.IO;
using System.Linq;

namespace Game
{
    internal static class FileManager
    {
        private static string filePath = "playerdata.txt";
        public static bool PlayerExists(string name)
        {
            if (!File.Exists(filePath))
                return false;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (line.StartsWith(name + ","))
                    return true;
            }

            return false;
        }

        public static int GetPlayerLevel(string name)
        {
            if (!File.Exists(filePath))
                return 0;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[0] == name)
                {
                    int level = 0;
                    int.TryParse(parts[1], out level);
                    return level;
                }
            }

            return 0;
        }

        public static void SavePlayer(string name, int levelCompleted, int score)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            bool playerFound = false;

            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts[0] == name)
                {
                    int oldLevel = 0;
                    int.TryParse(parts[1], out oldLevel);

                    if (levelCompleted > oldLevel)
                    {
                        lines[i] = name + "," + levelCompleted + "," + score;
                    }
                    playerFound = true;
                    break;
                }
            }

            if (!playerFound)
            {
                lines.Add(name + "," + levelCompleted + "," + score);
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
