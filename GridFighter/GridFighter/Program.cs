using System;

namespace GridFighter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainGameClass game = new MainGameClass())
            {
                game.Run();
            }
        }
    }
#endif
}

