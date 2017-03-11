using SkyEditor.ExtensionWebsite.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite
{
    public static class ExtensionsMonitor
    {
        private static FileSystemWatcher watcher;

        private static ExtensionsConfig config;

        public static bool HasStarted { get; private set; }

        public static void StartExtensionMonitor(ExtensionsConfig config)
        {
            HasStarted = true;
            var path = Path.Combine(config.ExtensionsPath, "In");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            ExtensionsMonitor.config = config;

            watcher = new FileSystemWatcher(path);
            watcher.Created += watcher_Created;
            watcher.EnableRaisingEvents = true;
        }

        private static async void watcher_Created(object sender, FileSystemEventArgs e)
        {
            await ExtensionsHelper.InstallExtension(e.FullPath, config);
            File.Delete(e.FullPath);
        }

    }
}
