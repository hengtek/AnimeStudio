using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AnimeStudio
{
    public static class UnityCNManager
    {
        static UnityCNManager() {}

        public static void SetKey(UnityCN.Entry entry)
        {
            if (UnityCN.SetKey(entry))
            {
                Logger.Info($"[UnityCN] Selected Key is {entry.Name} - {entry.Key}");
            }
            else
            {
                Logger.Info($"[UnityCN] No Key is selected !!");
            }
        }
    }
}
