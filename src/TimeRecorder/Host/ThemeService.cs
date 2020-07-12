using MaterialDesignColors;
using MaterialDesignColors.Recommended;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TimeRecorder.Host
{
    static class ThemeService
    {
        public static Swatch CurrentTheme { get; private set; }

        static ThemeService()
        {
            CurrentTheme = new SwatchesProvider().Swatches.FirstOrDefault(s => s.Name == "deeppurple");
        }

        public static void ApplyFromName(string name )
        {
            var target = new SwatchesProvider().Swatches.FirstOrDefault(s => s.Name == name);
            if(target != null)
            {
                ApplyTheme(target);
                CurrentTheme = target;
            }
        }

        public static void ApplyTheme(Swatch swatch)
        {
            ApplyPrimary(swatch);
            ApplyAccent(swatch);
        }

        private static void ApplyPrimary(Swatch swatch)
        {
            if (swatch == null)
                return;

            ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));
        }

        private static void ApplyAccent(Swatch swatch)
        {
            if (swatch == null)
                return;

            ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
