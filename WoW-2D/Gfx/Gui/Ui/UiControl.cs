using Microsoft.Xna.Framework.Graphics;
using MonoGameHelper.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// A class for ui types.
    /// </summary>
    public abstract class UiControl : IGuiControl
    {
        public bool IsVisible { get; set; }

        public UiControl(GraphicsDevice graphics) : base(graphics) { }

        public virtual void Activator() {}
    }
}
