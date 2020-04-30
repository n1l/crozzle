using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrozzleDesktopApp
{
  public partial class MainForm : Form
  {
    private readonly IPresenter _presenter;
    
    public MainForm(IPresenter presenter)
    {
      _presenter = presenter;
      InitializeComponent();
      presenter.SetView(this);
    }

    private void LoadConfigMenuItemClick(object sender, EventArgs e)
    {
      _presenter.ShowDialog(true);
      _presenter.LoadConfig();
    }

    private void LoadCrozzleMenuItemClick(object sender, EventArgs e)
    {
      _presenter.ShowDialog();
      _presenter.RenderHtml();
    }




    public WebBrowser Browser
    {
      get { return WebBrowser; }
    }
  }
}
