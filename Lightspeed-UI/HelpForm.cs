using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Lightspeed_UI
{
    public class HelpForm : Form
    {
        public HelpForm(string helpText)
        {
            this.Text = "Lightspeed Shortcuts Guide";
            this.Size = new Size(450, 650);
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Cleaner look
            this.TopMost = true;

            // Make it draggable
            this.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); } };

            Panel header = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(40, 40, 40) };
            Label title = new Label 
            { 
                Text = "🚀 LIGHTSPEED SHORTCUTS", 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            header.Controls.Add(title);

            TextBox textBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 20, 20),
                ForeColor = Color.FromArgb(200, 200, 200),
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 10.5f),
                Text = helpText.Replace("`n", Environment.NewLine).Replace("\\n", Environment.NewLine),
                ScrollBars = ScrollBars.Vertical
            };

            Panel footer = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(30, 30, 30) };
            Button closeBtn = new Button
            {
                Text = "CLOSE",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            footer.Controls.Add(closeBtn);

            this.Controls.Add(textBox);
            this.Controls.Add(header);
            this.Controls.Add(footer);

            // Prevent default selection
            this.Load += (s, e) => {
                textBox.SelectionStart = 0;
                textBox.SelectionLength = 0;
                closeBtn.Focus(); // Move focus to button to avoid selecting text
            };

            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
            textBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
    }
}
