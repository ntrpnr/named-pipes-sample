using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaskbarCornerCustomizer.Shared;
using TaskbarCornerCustomizer.Shared.Models;

namespace NamedPipesSample.WindowsService
{
    internal class TrayIconService : IDisposable
    {
        private TrayManager? trayManager;
        private const string TRAY_ICON_PROCESS_NAME = "NamedPipesSample.TrayIcon";


        public TrayIconService()
        {
            try
            {
                var tccFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TCC");
                trayManager = new TrayManager(folderPath: tccFolderPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not start tray manager: {ex}");
            }
        }

        public void ShowTrayIcon()
        {
            var usersSids = trayManager.GetCurrentlyLoggedOnUsersSids().ToList();

            foreach (var userSid in usersSids)
            {
                trayManager.SetConfig(userSid, new Dictionary<string, TrayIconState>() { { TRAY_ICON_PROCESS_NAME, TrayIconState.Visible } });
            }
        }

        public void HideTrayIcon()
        {
            var usersSids = trayManager.GetCurrentlyLoggedOnUsersSids().ToList();

            foreach (var userSid in usersSids)
            {
                trayManager.SetConfig(userSid, new Dictionary<string, TrayIconState>() { { TRAY_ICON_PROCESS_NAME, TrayIconState.Hidden } });
            }
        }

        public void Dispose()
        {
            trayManager?.Dispose();
        }
    }
}