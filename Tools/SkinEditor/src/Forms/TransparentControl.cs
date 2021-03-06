#region Copyright (C) 2005-2008 Team MediaPortal

/* 
 *	Copyright (C) 2005-2008 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Drawing;

namespace SkinEditor.Forms
{
	/// <summary>
	/// Summary description for TransparentControl.
	/// </summary>
	public class TransparentControl : TranspControl.TranspControl
	{
		private Bitmap bitmap;
		public TransparentControl() {
			bitmap = new Bitmap("d:\\Tools\\MediaPortal\\skin\\mce\\Media\\alarm_clock.png");
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			e.Graphics.DrawImage(bitmap,0,0,bitmap.Width,bitmap.Height);
			//e.Graphics.DrawRectangle(new Pen(Color.Black,1.0f),0,0,Width-1,Height-1);
		}

	}
}
