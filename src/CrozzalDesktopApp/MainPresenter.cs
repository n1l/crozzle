using System;
using System.IO;
using System.Windows.Forms;
using CrozzleEngine.Exceptions;
using CrozzleEngine.Interfaces;

namespace CrozzleDesktopApp
{
  /// <summary>
  /// Presenter, for form controlling
  /// </summary>
  public class MainPresenter : IPresenter
  {
    //crozzle loader
    private readonly ICrozzleLoader<IConfiguration> _crozzleLoader;
    //config loader
    private readonly IModelLoader<IConfiguration> _configLoader;

    private MainForm _view;
    private string _previosDirectory;
    private string _configPath;
    private string _crozzlePath;
    private IConfiguration _config;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="crozzleLoader">Crozzle loader object, need to load crozzle from file</param>
    /// <param name="configLoader">Configuration loader object, need to load config from file</param>
    public MainPresenter(ICrozzleLoader<IConfiguration> crozzleLoader, IModelLoader<IConfiguration> configLoader)
    {
      _crozzleLoader = crozzleLoader;
      _configLoader = configLoader;
    }

    /// <summary>
    /// Set controlling form
    /// </summary>
    /// <param name="view">Form for controlling <see cref="MainForm"/></param>
    public void SetView(Form view)
    {
      _view = (MainForm)view;
      _previosDirectory = Directory.GetCurrentDirectory();
    }

    private void LoadHtml(ICrozzle crozzle)
    {
      var html = crozzle.SerializeToHtml();
      _view.Browser.DocumentText = html;
    }

    /// <summary>
    /// Create html for browsing
    /// </summary>
    public void RenderHtml()
    {
      try
      {
        _crozzleLoader.SetConfig(_config);
        _crozzleLoader.CreateModel(_crozzlePath);
        var crozzle = _crozzleLoader.GetModel();
        LoadHtml(crozzle);
      }
      catch (ModelLoadException ex)
      {
        MessageBox.Show(_view, ex.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show(_view, ex.ToString());
      }

    }

    /// <summary>
    /// Load configuration
    /// </summary>
    public void LoadConfig()
    {
      try
      {
        _configLoader.CreateModel(_configPath);
        _config = _configLoader.GetModel();
        if (_config != null && _config.ValidationResult.IsValid)
        {
          MessageBox.Show("Configuration loaded successfully");
        }
        else
        {
          MessageBox.Show("Configuration has error");
        }
      }
      catch (ModelLoadException ex)
      {
        MessageBox.Show(_view, ex.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show(_view, ex.ToString());
      }

    }

    /// <summary>
    /// Show file loader fialog
    /// </summary>
    /// <param name="configNeed">Config or crozzle to loading</param>
    public void ShowDialog(bool configNeed = false)
    {
      var dialog = new OpenFileDialog { DefaultExt = ".txt", InitialDirectory = _previosDirectory };
      var result = dialog.ShowDialog();
      if (result != DialogResult.OK)
      {
        return;
      }
      _previosDirectory = Path.GetDirectoryName(dialog.FileName);
      if (configNeed)
      {
        _configPath = dialog.FileName;
      }
      else
      {
        _crozzlePath = dialog.FileName;
      }
    }
  }
}
