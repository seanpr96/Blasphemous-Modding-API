using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Modding
{
    internal static class ModLoader
    {
        public static bool Loaded { get; private set; }
        public static bool Initialized { get; private set; }

        public static readonly Dictionary<string, Mod> LoadedMods = new Dictionary<string, Mod>();

        internal static void LoadMods()
        {
            if (Loaded)
            {
                return;
            }

            Logger.API.LogVerbose("Beginning " + nameof(LoadMods));

            // Determine where to place mods based on current OS
            string path = string.Empty;
            if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                path = Application.dataPath + @"\Managed\Mods";
            }
            else if (SystemInfo.operatingSystem.Contains("Mac"))
            {
                path = Application.dataPath + "/Resources/Data/Managed/Mods/";
            }
            else if (SystemInfo.operatingSystem.Contains("Linux"))
            {
                path = Application.dataPath + "/Managed/Mods";
            }

            if (path == string.Empty)
            {
                Logger.API.LogFatal($"Unknown operating system \"{SystemInfo.operatingSystem}\", cannot load mods.");
                Loaded = true;
                return;
            }

            Logger.API.LogInfo("===============BEGINNING PRE-INITIALIZATION===============");

            // Iterate over all dll files in mods folder
            foreach (string modFileName in Directory.GetFiles(path, "*.dll"))
            {
                Logger.API.LogDebug("Loading mods from file " + modFileName);

                try
                {
                    foreach (Type type in Assembly.LoadFile(modFileName).GetExportedTypes())
                    {
                        if (!type.IsSubclassOf(typeof(Mod)))
                        {
                            continue;
                        }

                        // Check for a paramaterless contructor on any found mods
                        ConstructorInfo ctor = type.GetConstructor(new Type[0]);

                        if (ctor == null)
                        {
                            Logger.API.LogWarn(
                                $"Mod {type.Name} contains no parameterless constructor and cannot be loaded");
                            continue;
                        }

                        Mod mod = (Mod)ctor.Invoke(new object[0]);

                        // Ensure that mods have unique IDs
                        string id = mod.GetModID();
                        if (LoadedMods.ContainsKey(id))
                        {
                            Logger.API.LogWarn($"Duplicate mod ID \"{id}\", skipping");
                            continue;
                        }

                        // Attempt to load the mod
                        mod.PreInitialize();
                        LoadedMods[id] = mod;

                        Logger.API.LogInfo($"Mod {id} pre-initialization complete");
                    }
                }
                catch (Exception e)
                {
                    Logger.API.LogError($"Error loading mods from file \"{modFileName}\":\n{e}");
                }
            }

            Loaded = true;
        }

        public static void InitializeMods()
        {
            if (Initialized)
            {
                return;
            }

            Logger.API.LogInfo("===============BEGINNING INITIALIZATION===============");

            foreach ((string id, Mod mod) in LoadedMods)
            {
                try
                {
                    mod.Initialize();
                }
                catch (Exception e)
                {
                    Logger.API.LogError($"Error initializing mod \"{id}\"\n{e}");
                }
            }

            Logger.API.LogInfo("===============BEGINNING POST-INITIALIZATION===============");

            foreach ((string id, Mod mod) in LoadedMods)
            {
                try
                {
                    mod.PostInitialize();
                }
                catch (Exception e)
                {
                    Logger.API.LogError($"Error post-initializing mod \"{id}\"\n{e}");
                }
            }

            Initialized = true;
        }
    }
}
