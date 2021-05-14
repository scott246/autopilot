using System.Collections.Generic;
using System.Windows.Input;

namespace autopilot.Objects
{
	public class Bind
	{
		public Bind(List<Key> keys)
		{
			Keys = keys;
		}
		public List<Key> Keys { get; set; }
	}
}
