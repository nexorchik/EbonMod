﻿using System;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EbonianMod
{
    internal class LocalizationRewriter : ModSystem //thanks to scalie for this absolute godsend
    {
        public override void PostSetupContent()
        {
#if DEBUG
            MethodInfo refreshInfo = typeof(LocalizationLoader).GetMethod("UpdateLocalizationFilesForMod", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(GameCulture) });
            refreshInfo.Invoke(null, new object[] { EbonianMod.Instance, null, Language.ActiveCulture });
#endif
        }
    }

    internal static class LocalizationRoundabout
    {
        public static void SetDefault(this LocalizedText text, string value)
        {
#if DEBUG
            PropertyInfo valueProp = typeof(LocalizedText).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

            LanguageManager.Instance.GetOrRegister(text.Key, () => value);
            valueProp.SetValue(text, value);
#endif
        }
    }
}