using System.Drawing;
namespace Data_sience
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            int x = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            this.ClientSize = new System.Drawing.Size(x, y);
            this.BackColor = Color.Black;
            this.Text = "Form1";

        }

        #endregion
    }
}

