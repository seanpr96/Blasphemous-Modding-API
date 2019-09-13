using JetBrains.Annotations;
using Modding;

namespace ExampleMod1
{
    [PublicAPI]
    public class ExampleMod1 : Mod
    {
        // The string given to the base constructor controls the name prepended to debug output from this mod
        public ExampleMod1() : base("Example Mod 1") { }

        protected override void PreInitialize()
        {
            // This method is called before any game initialization code is run
            // Other mods could also not have been loaded into the game, depending on the order of pre-init calls
            // This is a good spot to run initialization steps that don't depend on the game or other mods

            Logger.LogVerbose("Hello world!");
        }

        protected override void Initialize()
        {
            // This method is called after game initialization, but it is indeterminate if other mods have been loaded
            // This is a good spot to do the main bulk of your mod's initialization, such as hooking onto game events
        }

        protected override void PostInitialize()
        {
            // At this point, all other mods are guaranteed to have been loaded
            // This is a good spot to run any code that relies on other mods
        }

        public override string GetModID()
        {
            // Must return a unique identifier for your mod
            // Including your name in the identifier is a good way of ensuring this
            return "seanpr:ExampleMod1";
        }
    }
}
