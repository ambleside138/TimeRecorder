using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Configurations.Items
{
    class ThemeConfig
    {
        public string ThemeName { get; set; }

        public ThemeConfig(Swatch swatch)
        {
            ThemeName = swatch.Name;
        }

        public ThemeConfig()
        {

        }
    }
}
