using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Data_sience
{
    public partial class Form1 : Form
    {
        private Button m_Button;
        private CheckBox m_CheckBox;
        private Graphics m_Graphics;
        private Rectangle m_Rectangle;
        private DrawSupport m_DrawSupport;
        private bool m_DoPaintText;

        public Form1()
        {
            InitializeComponent();
            m_Graphics = CreateGraphics();
            m_CheckBox = new CheckBox();
            m_DoPaintText = false;
            Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (m_DoPaintText)
            {
                DrawText();
            }
        }

        private void DrawText()
        {
            Font font = new Font("Arial", 16f);
            Point point = new Point(1000, 10);
            m_DrawSupport.DrawText("Hi my name is steven", font, point, Color.White);
        }

        private void Start()
        {
            m_DrawSupport = new DrawSupport(m_Graphics);
            PaintButton();
        }

        private void PaintButton()
        {
            m_Button = new Button();
            Point point = new Point(500, 500);
            Size size = new Size(point);
            m_Button.Location = point;
            m_Button.Size = size;
            m_Button.Click += Button1CLick;
            this.Controls.Add(m_DrawSupport.CreateButton(m_Button, ButtonState.Flat));
        }

        private void SetText(object sender, EventArgs e)
        {
            m_DoPaintText = true;
            OnPaint(new PaintEventArgs(m_Graphics, CreateRectangle(0, 0, 1980, 1080)));
        }

        private void Button1CLick(object sender, EventArgs e)
        {
            m_CheckBox.Location = new Point(100, 100);
            m_CheckBox.Size = new Size(100, 100);
            m_CheckBox.AutoSize = true;
            m_CheckBox.CheckedChanged += SetText;
            ControlPaint.DrawCheckBox(Graphics.FromHwnd(m_CheckBox.Handle), CreateRectangle(m_CheckBox.Location, m_CheckBox.Size), ButtonState.Flat);
            Controls.Add(m_CheckBox);
        }

        private Rectangle CreateRectangle(int x, int y, int sizeX, int sizeY)
        {
            return new Rectangle(x, y, sizeX, sizeY);
        }

        private Rectangle CreateRectangle(Point point, Size size)
        {
            return new Rectangle(point, size);
        }
    }
}