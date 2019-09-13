using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Modding
{
    [PublicAPI]
    public class Logger
    {
        // TODO: Config to allow changing log level
        internal static LogLevel Level { get; set; } = LogLevel.Verbose;
        internal static readonly Logger API = new Logger("API");

        private static readonly StreamWriter Writer;
        private readonly string _name;

        static Logger()
        {
            // Create path string for logging
            string pathSeparator = SystemInfo.operatingSystem.Contains("Windows") ? @"\" : "/";
            string logFolder = Application.persistentDataPath + pathSeparator + "Mod logs" + pathSeparator;
            string dateTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
            string fullPath = logFolder + $"ModLog ({dateTime}).txt";

            // Create folder for logs
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            // Failsafe check for file somehow already existing
            if (File.Exists(fullPath))
            {
                int i = 1;
                while (File.Exists(fullPath + $" ({i})"))
                {
                    i++;
                }

                fullPath += $" ({i})";
            }

            Writer = new StreamWriter(new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite),
                Encoding.UTF8) {AutoFlush = true};
        }

        public Logger(string name)
        {
            _name = name;
        }

        public void Log(string message, LogLevel level)
        {
            // Ignore messages with too low of a logging level
            if (level < Level)
            {
                return;
            }

            string time = DateTime.Now.ToString("HH:mm:ss");

            lock (Writer)
            {
                Writer.WriteLine($"[{time}] [{level.ToString()}] [{_name}] - {message}");
            }
        }

        public void LogVerbose(string message) => Log(message, LogLevel.Verbose);
        public void LogDebug(string message) => Log(message, LogLevel.Debug);
        public void LogInfo(string message) => Log(message, LogLevel.Info);
        public void LogWarn(string message) => Log(message, LogLevel.Warn);
        public void LogError(string message) => Log(message, LogLevel.Error);
        public void LogFatal(string message) => Log(message, LogLevel.Fatal);

        public void Log(object message, LogLevel level) => Log(ToStringSafe(message), level);
        public void LogVerbose(object message) => Log(message, LogLevel.Verbose);
        public void LogDebug(object message) => Log(message, LogLevel.Debug);
        public void LogInfo(object message) => Log(message, LogLevel.Info);
        public void LogWarn(object message) => Log(message, LogLevel.Warn);
        public void LogError(object message) => Log(message, LogLevel.Error);
        public void LogFatal(object message) => Log(message, LogLevel.Fatal);

        private static string ToStringSafe(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            try
            {
                return obj.ToString();
            }
            catch (Exception e)
            {
                return e.GetType().Name;
            }
        }

        public enum LogLevel
        {
            Verbose,
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
            [UsedImplicitly] Off
        }
    }
}
