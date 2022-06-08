namespace PokeViewer.NET
{
    static class Program
    {
        public static MainViewer? Viewer;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Viewer = new MainViewer();
            Application.Run(Viewer);
        }
    }
}