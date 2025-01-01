using Lightspeed_UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lightspeed_UI
{
    public partial class Form1 : Form
    {
        private WebBrowser webBrowser;
        Panel buttonPanel;
        // 顶部间距
        public int tophieght = 56;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            this.Width = 410;
            this.Height = 545 + tophieght;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";

            if (Settings.Default.autobutton5)
            {
                checkbox1.Checked = true;
                button4_Click(button4, EventArgs.Empty);
            }

            if (!Directory.Exists("c:/lightspeed"))
            {
                OpenSettings(button4, EventArgs.Empty);
                label1.Text = "第一次使用,需要建立文件夹 C:/lightspeed,点击左边按钮创建 ";
                label1.ForeColor = Color.Red;

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = Settings.Default.opicity;
        }
        private void InitializeUI()
        {
            // 设置窗体的黑色背景
            this.BackColor = Color.Black;

            // 创建一个 Panel 用于放置按钮，并设置为黑色背景
            buttonPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 100,
                BackColor = Color.FromArgb(30, 30, 30)  // 深灰色背景
            };

            // 添加向上一级按钮
            Button backButton = new Button
            {
                Text = " < ",
                Width = 45,
                Height = 40,
                Top = 10,
                Left = 5,
                BackColor = Color.FromArgb(45, 45, 45), // 深灰色背景
                ForeColor = Color.White  // 白色字体
            };
            backButton.FlatStyle = FlatStyle.Flat;  // 扁平样式
            backButton.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);  // 边框颜色
            backButton.Click += button6_Click; // 添加按钮点击事件处理程序
            buttonPanel.Controls.Add(backButton);
            // 添加向下一级级按钮
            Button forwardButton = new Button
            {
                Text = " > ",
                Width = 45,
                Height = 40,
                Top = 10,
                Left = 50,
                BackColor = Color.FromArgb(45, 45, 45), // 深灰色背景
                ForeColor = Color.White  // 白色字体
            };
            forwardButton.FlatStyle = FlatStyle.Flat;  // 扁平样式
            forwardButton.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);  // 边框颜色
            forwardButton.Click += button7_Click; // 添加按钮点击事件处理程序
            buttonPanel.Controls.Add(forwardButton);

            // 创建 10 个按钮，并设置为黑色背景和白色字体
            for (int i = 0; i <= 9; i++)
            {
                Button btn = new Button
                {
                    Text = i.ToString(),
                    Tag = i,  // 将按钮的 Tag 设置为数字，用于点击事件中获取
                    Width = 90,
                    Height = 40,
                    Top = i * 45 + tophieght, // 设置按钮的垂直位置
                    Left = 5,
                    BackColor = Color.FromArgb(45, 45, 45), // 深灰色背景
                    ForeColor = Color.White  // 白色字体
                };
                btn.FlatStyle = FlatStyle.Flat;  // 扁平样式
                btn.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);  // 边框颜色
                btn.Click += Btn_Click; // 添加按钮点击事件处理程序
                buttonPanel.Controls.Add(btn);
            }
            Button btn_last = new Button
            {
                Text = "settings",
                Width = 90,
                Height = 40,
                Top = 450 + tophieght, // 设置按钮的垂直位置
                Left = 5,
                BackColor = Color.FromArgb(45, 45, 45), // 深灰色背景
                ForeColor = Color.White  // 白色字体
            };
            btn_last.FlatStyle = FlatStyle.Flat;  // 扁平样式
            btn_last.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);  // 边框颜色
            btn_last.Click += OpenSettings; // 添加按钮点击事件处理程序
            buttonPanel.Controls.Add(btn_last);

            // 创建 WebBrowser 控件
            webBrowser = new WebBrowser
            {

                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true,  // 抑制 JavaScript 错误弹窗
            };

            // 将 Panel 和 WebBrowser 控件添加到窗体
            this.Controls.Add(webBrowser);
            this.Controls.Add(buttonPanel);


            // 设置初始页面
            webBrowser.Navigate("file://C:/lightspeed/0");
            webBrowser.PreviewKeyDown += WebBrowserKeyFunction;
            
        }
        private void WebBrowserKeyFunction(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Alt == true &&  e.KeyCode == Keys.Left) )
            {
                if (webBrowser.CanGoBack)
                {
                    webBrowser.GoBack();
                    webBrowser.Focus();
                }
                
            }
            if ((e.Alt == true && e.KeyCode == Keys.Right))
            {
                if (webBrowser.CanGoForward)
                {
                    webBrowser.GoForward();
                    webBrowser.Focus();
                }

            }
            if ((e.KeyCode == Keys.F5))//按一下 f5刷新 ahk 重启一下
            {
                MessageBox.Show("lightspeed.ahk 已刷新");
                button4_Click(button4, EventArgs.Empty);
            }

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            webBrowser.Visible = true;

            // 获取被点击按钮的 Tag 值
            Button clickedButton = sender as Button;
            //更新按钮颜色
            buttonRefreshColor(clickedButton);
            int number = (int)clickedButton.Tag;
            // 如果正在开启设置页面,关闭
            panel1.Visible = false;

            // 更新 WebBrowser 控件的 URI
            webBrowser.Navigate($"file://C:/lightspeed/{number}");
        }

        private void buttonRefreshColor(Button clickedButton)
        {
            //帮我完成这个函数
            //要求 传入的这个btn clickedButton.FlatAppearance.BorderColor = Color.FromArgb(255, 255, 64);  
            //其他的所有 button FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            // 遍历 Panel 中的所有按钮
            foreach (Control ctrl in buttonPanel.Controls)
            {
                // 检查控件是否是按钮
                if (ctrl is Button btn)
                {
                    // 如果当前按钮是被点击的按钮，设置边框颜色为黄色
                    if (btn == clickedButton)
                    {
                        btn.FlatAppearance.BorderColor = Color.FromArgb(255, 255, 64);  // 设置为黄色
                    }
                    else
                    {
                        // 否则，设置为灰色边框
                        btn.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);  // 设置为灰色
                    }
                }
            }
        }

        private void OpenSettings(object sender, EventArgs e)
        {

            //var b = new Form2();
            //b.Show();
            if (!panel1.Visible)
            {
                //min 460x590
                //this.Width = Math.Max(460+120, Width);
                this.Height = Math.Max(590, Height);
                panel1.Visible = true;
                panel1.Top = webBrowser.Top;
                panel1.Height = webBrowser.Height;
                panel1.Left = webBrowser.Left;
                panel1.Width = webBrowser.Width;
                webBrowser.Visible = false;
            }
            else
            {
                panel1.Visible = false;
                webBrowser.Visible = true;

            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            string baseFolderPath = @"c:/lightspeed";
            if (!Directory.Exists(baseFolderPath))
            {
                Directory.CreateDirectory(baseFolderPath);
                for (int i = 0; i < 10; i++)
                {
                    string folderPath = Path.Combine(baseFolderPath, i.ToString());
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
                label1.Text = ("文件夹创建完成！");
            }
            else
            {
                label1.Text = ("文件夹或许已存在?>无需创建");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateAHK("C:\\lightspeed");
            label2.Text = "ahk已生成,C:\\lightspeed\\lightspeed.ahk,包含应用数量:" + LoadFolder2objList("C:\\lightspeed").Count.ToString();  //todo ,懒得优化了,总之这个函数执行了两遍
        }
        public void CreateAHK(string folderPath)
        {
            // Define the path to the AHK file
            string ahkFilePath = Path.Combine(folderPath, "lightspeed.ahk");

            // AHK script content
            string ahkContent = @"
#SingleInstance , Force
#If WinActive(""ahk_class Shell_TrayWnd"") or WinActive(""ahk_class Shell_SecondaryTrayWnd"") or WinActive(""python  lightspeed.py"") or WinActive(""ahk_class WorkerW"")  or WinActive(""ahk_class Progman"")
SetTitleMatchMode, 2

ShowAndHideText(text, duration) {
    Gui, +LastFound +AlwaysOnTop -Caption +ToolWindow +Disabled
    Gui, Color, 000000 ; background black
    Gui, Font, s15, Verdana ; fontsize and fontname

    textWidth := 400
    textHeight := 40
    winX := 0
    winY := 20

    Gui, Add, Text, x%winX% y%winY% w%textWidth% h%textHeight% cFFFFFF Center, %text%
    Gui, Show, NA
    WinSet, Transparent, 180 ; 0 is fully transparent, 255 is fully opaque

    SetTimer, DestroyGui, %duration%
    return

    DestroyGui:
    Gui, Destroy
    return
}

open_or_activate(title, path) {
    ShowAndHideText(title, 600)
    if (WinExist(title)) {
        WinActivate, %title%
    } else {
        Run, ""%path%""
        }
}

!0::
open_or_activate(""C:\lightspeed\0"",""C:\lightspeed\0"")
return
";

            ahkContent += $"^!z::open_or_activate(\"lightspeed-UI\",\"{Assembly.GetExecutingAssembly().Location}\")" + "\n";

            var lightspeed_obj_list = LoadFolder2objList(folderPath);
            foreach (var item in lightspeed_obj_list)
            {
                ahkContent += (item.getAhkString());
                ahkContent += "\n";
            }

            // Write the AHK script content to the file
            System.IO.File.WriteAllText(ahkFilePath, ahkContent, Encoding.GetEncoding("GBK"));


        }

        public List<lightspeed_obj> LoadFolder2objList(string folderPath)
        {
            var lightspeed_obj_list = new List<lightspeed_obj>();
            var dubcheckList = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                var tf = folderPath + "\\" + i;
                var files = Directory.GetFiles(tf);

                foreach (var file in files)
                {
                    var hotkey = "";
                    var title = "";
                    if (Path.GetFileName(file) == "desktop.ini") continue;

                    if (Path.GetFileName(file)[0] >= 'A' && Path.GetFileName(file)[0] <= 'Z' || Path.GetFileName(file)[0] >= 'a' && Path.GetFileName(file)[0] <= 'z')
                    {
                        hotkey = i + " & " + char.ToLower(Path.GetFileName(file)[0]);
                    }
                    else
                    {
                        Console.WriteLine($"nope{Path.GetFileName(file)}");
                        continue;
                    }

                    if (Path.GetFileName(file).StartsWith("["))
                    {
                        hotkey = i + " & " + char.ToLower(Path.GetFileName(file)[1]);
                        title = Path.GetFileName(file).Substring(3).Replace(".lnk", "").Replace(" - 快捷方式", "").Replace(" - 副本", "");
                    }

                    if (dubcheckList.Contains(hotkey))
                    {
                        Console.WriteLine($"{Path.GetFileName(file)} :dublicated key");
                        continue;
                    }

                    dubcheckList.Add(hotkey);
                    title = System.IO.Path.GetFileNameWithoutExtension(file).Replace(" - 快捷方式", "").Replace(" - 副本", "");
                    var filepath = Path.Combine(folderPath, file);
                    lightspeed_obj_list.Add(new lightspeed_obj(title, filepath, hotkey));

                }
                //MessageBox.Show("Test" + lightspeed_obj_list.Count);
            }

            return lightspeed_obj_list;
        }

        public static void StartProgram(string filePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error starting program: {e.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartProgram("C:\\lightspeed\\lightspeed.ahk");
            label3.Text = "执行 lightspeed.ahk";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2_Click(button4, EventArgs.Empty);
            button3_Click(button4, EventArgs.Empty);
            label4.Text = "火箭升空";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://kasusa.lanzoul.com/iClHk28thruj";

            // 使用默认浏览器打开网页
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/cornradio/lightspeed/tree/main/seticons";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

        }

        private void checkbox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.autobutton5 = checkbox1.Checked;
            Settings.Default.Save();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            string url = "https://kasusa.lanzoul.com/ix6FC28v6xyf";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void labelx_Click(object sender, EventArgs e)
        {

        }
        //back
        private void button6_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
                webBrowser.Focus();
            }
        }
        //forward
        private void button7_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
                webBrowser.Focus();
            }
        }


        //big
        private void button8_Click(object sender, EventArgs e)
        {
            this.Width = 850;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Width = 450;
        }


        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/cornradio/lightspeed-UI";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.Opacity = trackBar1.Value/100.00;
            label_opacity.Text = trackBar1.Value.ToString();
            Settings.Default.opicity = this.Opacity;
            Settings.Default.Save();
        }
    }
}
