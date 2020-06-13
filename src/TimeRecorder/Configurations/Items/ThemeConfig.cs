using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TimeRecorder.Configurations.Items
{
    /// <summary>
    /// テーマ色設定を表します
    /// </summary>
    class ThemeConfig : ConfigItemBase
    {
        public string ThemeName { get; set; }

        public override ConfigKey Key => ConfigKey.Theme;

        public ThemeConfig(Swatch swatch)
        {
            ThemeName = swatch.Name;
        }

        public ThemeConfig()
        {

        }
    }
}
