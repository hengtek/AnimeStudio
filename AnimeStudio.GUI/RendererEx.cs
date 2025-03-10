using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimeStudio.GUI
{
    public class RendererEx : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var item = e.Item;

            item.ForeColor = System.Drawing.Color.White;

            if (item.Selected)
            {
                e.Item.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
            }
            else
            {
                e.Item.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            }

            base.OnRenderMenuItemBackground(e);
        }
    }
}
