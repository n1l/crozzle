using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrozzleDesktopApp
{
    public interface IPresenter
    {
        void SetView(Form view);
        void RenderHtml();
        void LoadConfig();
        void ShowDialog(bool configNeed = false);
    }
}
