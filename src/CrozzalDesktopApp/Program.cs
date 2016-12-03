using System;
using System.Windows.Forms;
using CrozzleEngine.Loaders;
using CrozzleEngine.Validators;

namespace CrozzleDesktopApp
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(Configure());
    }

    /// <summary>
    /// Sample IoC container
    /// </summary>
    /// <returns>Configured form for running</returns>
    private static Form Configure()
    {
      var crozzleFileValidator = new CrozzleFileValidator();
      var configFileValidator = new ConfigurationFileValidator();
      var crozzleLoader = new CrozzleLoader(crozzleFileValidator);
      var configLoader = new ConfigurationLoader(configFileValidator);
      var presenter = new MainPresenter(crozzleLoader, configLoader);
      return new MainForm(presenter);
    }
  }
}
