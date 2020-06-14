using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Controls.WindowLocation
{
	public interface IWindowSettings
	{
		WINDOWPLACEMENT? Placement { get; set; }
		void Reload();
		void Save();
	}
}
