namespace InformSearch;

static class Program
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
        
        // Create a form selector to choose between original search algorithms and TSP
        var selectorForm = new MainSelector();
        Application.Run(selectorForm);
    }    
}