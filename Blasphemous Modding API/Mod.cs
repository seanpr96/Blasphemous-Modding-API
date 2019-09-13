using JetBrains.Annotations;

namespace Modding
{
    public abstract class Mod
    {
        protected readonly Logger Logger;

        protected Mod(string name)
        {
            Logger = new Logger(name);
        }

        protected Mod() : this(null) { }

        [PublicAPI]
        public static bool IsModLoaded(string id)
        {
            return ModLoader.LoadedMods.ContainsKey(id);
        }

        [PublicAPI]
        public static bool TryGetMod(string id, out Mod mod)
        {
            return ModLoader.LoadedMods.TryGetValue(id, out mod);
        }

        public abstract string GetModID();

        protected internal virtual void PreInitialize()
        {
        }

        protected internal virtual void Initialize()
        {
        }

        protected internal virtual void PostInitialize()
        {
        }
    }
}
