using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace TimeRecorder.Controls;
internal class RunnableHyperlink : Hyperlink
{
	// https://github.com/Yuubari/LegacyKCV/blob/77cf85dff85a4c5a86ad44954764bfad70cbf2c8/Grabacr07.Desktop.Metro/Controls/HyperlinkEx.cs

	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	#region Uri 依存関係プロパティ

	public Uri Uri
	{
		get { return (Uri)this.GetValue(UriProperty); }
		set { this.SetValue(UriProperty, value); }
	}
	public static readonly DependencyProperty UriProperty =
		DependencyProperty.Register("Uri", typeof(Uri), typeof(RunnableHyperlink), new UIPropertyMetadata(null));

	#endregion

	protected override void OnClick()
	{
		base.OnClick();

		if (this.Uri == null)
			return;

		try
		{
			Process.Start(new ProcessStartInfo
			{
				UseShellExecute = true,
				FileName = Uri.ToString(),
			});
		}
		catch (Exception ex)
		{
			_logger.Error(ex);
		}
	}
}
