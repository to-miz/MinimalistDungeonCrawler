using System;

namespace MinimalisticDungeonCrawler
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MinimalGame game = new MinimalGame())
            {
                game.Run();
            }
        }
    }
#endif
}

