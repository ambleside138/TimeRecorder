using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeRecorder.Helpers;
using System.Windows;

namespace TimeRecorder.Controls.WindowLocation
{
    class JsonFileWindowSettings : IWindowSettings
    {
        private const string _FileName = "windowsetting.json";

        public WINDOWPLACEMENT? Placement { get; set; }

        public void Reload()
        {
            if (File.Exists(_FileName) == false)
                return;

            var config = JsonFileIO.Deserialize<WindowPlacementJSON>(_FileName);
            Placement = config?.ConvertToInteropStruct();
        }

        public void Save()
        {
            if (Placement.HasValue == false)
                return;

            JsonFileIO.Serialize(new WindowPlacementJSON(Placement.Value), _FileName);
        }


        private class WindowPlacementJSON
        {
            // JSONではpublicプロパティしかシリアライズ対象とならないため保存用にクラス定義が必要となった

            public int Length { get; set; }
            public int Flags { get; set; }
            public SW ShowCmd { get; set; }
            public System.Windows.Point MinPosition { get; set; }
            public System.Windows.Point MaxPosition { get; set; }
            public RectJSON NormalPosition { get; set; }

            public WindowPlacementJSON()
            {
                // need to deserialize
            }

            public WindowPlacementJSON(WINDOWPLACEMENT value)
            {
                Length = value.length;
                Flags = value.flags;
                ShowCmd = value.showCmd;
                MinPosition = new Point(value.minPosition.X, value.minPosition.Y);
                MaxPosition = new Point(value.maxPosition.X, value.maxPosition.Y);
                NormalPosition = new RectJSON(value.normalPosition);
            }

            public WINDOWPLACEMENT ConvertToInteropStruct()
            {
                return new WINDOWPLACEMENT
                {
                    length = Length,
                    flags = Flags,
                    showCmd = ShowCmd,
                    minPosition = new POINT((int)MinPosition.X, (int)MinPosition.Y),
                    maxPosition = new POINT((int)MaxPosition.X, (int)MaxPosition.Y),
                    normalPosition = NormalPosition.ConvertToInteropStruct(),
                };
            }


        }

        public class RectJSON
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }

            public int Bottom { get; set; }

            public RectJSON()
            {
                // need to deserialize
            }

            public RectJSON(RECT rect)
            {
                Left = rect.Left;
                Top = rect.Top;
                Right = rect.Right;
                Bottom = rect.Bottom;
            }

            public RECT ConvertToInteropStruct()
            {
                return new RECT
                {
                    Left = Left,
                    Top = Top,
                    Right = Right,
                    Bottom = Bottom,
                };
            }
        }

    }

}
