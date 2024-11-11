using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMP_server.utils
{
    public class TrayIconManager
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public TrayIconManager()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("show console", null, ShowConsole);
            trayMenu.Items.Add("hide console", null, HideConsole);
            trayMenu.Items.Add("close server", null, Exit);

            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Information,
                ContextMenuStrip = trayMenu,
                Visible = true
            };

            trayIcon.DoubleClick += (sender, args) => ShowConsole(sender, args);
        }

        private void ShowConsole(object sender, EventArgs e)
        {
            ConsoleManager.ShowConsole();
            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(standardOutput);
        }
        private void HideConsole(object sender, EventArgs e)
        {
            ConsoleManager.HideConsole();
        }
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Dispose()
        {
            trayIcon.Dispose();
        }
    }
}
