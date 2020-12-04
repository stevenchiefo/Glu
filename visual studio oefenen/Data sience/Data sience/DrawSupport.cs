using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_sience
{
    public class DrawSupport
    {
        private Graphics m_Graphics;

        public DrawSupport(Graphics _graphics)
        {
            m_Graphics = _graphics;
        }

        public void DrawRectangle(Point point, Size size, Color color, int LineWidth)
        {
            Rectangle rectangle = new Rectangle(point, size);
            m_Graphics.DrawRectangle(GetPen(color, LineWidth), rectangle);
        }

        public Button CreateButton(Button _button, ButtonState _buttonState)
        {
            Rectangle _rectangle = new Rectangle(_button.Location, _button.Size);
            ControlPaint.DrawButton(Graphics.FromHwnd(_button.Handle), _rectangle, _buttonState);
            return _button;
        }

        public void DrawText(string s, Font font, Point point, Color color)
        {
            StringFormat drawFormat = new StringFormat();
            m_Graphics.DrawString(s, font, GetBrush(color), point, drawFormat);
        }

        private Pen GetPen(Color color, int LineWidth)
        {
            return new Pen(color, LineWidth);
        }

        private SolidBrush GetBrush(Color color)
        {
            return new SolidBrush(color);
        }
    }
}