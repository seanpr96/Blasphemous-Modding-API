using MonoMod;

// Missing dllimport on extern method
#pragma warning disable CS0626

namespace Modding.Patches
{
    [MonoModPatch("Framework.Managers.Core")]
    public class Core : Framework.Managers.Core
    {
        [MonoModOriginalName("Initialize")]
        public extern void orig_Initialize();

        public new void Initialize()
        {
            ModLoader.LoadMods();
            orig_Initialize();
            ModLoader.InitializeMods();
        }
    }
}
