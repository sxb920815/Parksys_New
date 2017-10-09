using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.WinForm
{
	public class C
	{

		public static string ST(int n)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < n; i++)
			{
				sb.Append("\t");
			}
			return sb.ToString();
		}
	}
}
