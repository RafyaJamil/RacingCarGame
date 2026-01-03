using Game.Component;
using Game.Core;
using Game.Audios;
namespace Game
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AudioManager.Init();        // ?? load all sounds
            AudioManager.Play("bgm");
            Application.Run(new MainForm());
        }
    }
}