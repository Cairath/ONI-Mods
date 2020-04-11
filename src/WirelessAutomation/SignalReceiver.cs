using UnityEngine;

namespace WirelessAutomation
{
	public class SignalReceiver
	{
		public int Id { get; set; }
		public int Channel { get; set; }
		public GameObject GameObject { get; set; }

		public SignalReceiver(int channel,GameObject go)
		{
			Channel = channel;
			GameObject = go;
		}
	}
}
