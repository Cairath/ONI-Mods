using System.Collections.Generic;
using System.Linq;
using KSerialization;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public static class WirelessAutomationManager
	{
		private static List<SignalEmitter> Emitters { get; } = new List<SignalEmitter>();

		public static void ResetEmittersList()
		{
			Emitters.Clear();
		}

		public static int RegisterEmitter(SignalEmitter emitter)
		{
			var newId = 0;
			if (Emitters.Count > 0)
			{
				newId = Emitters.Max(e => e.Id) + 1;
			}

			emitter.Id = newId;
			Emitters.Add(emitter);

			Debug.Log($"registered emitter with id {newId}, total registered: {Emitters.Count}");

			return emitter.Id;
		}

		public static void UnregisterEmitter(int emitterId)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);
			Emitters.Remove(emitter);
			Debug.Log($"unregistered emitter with id {emitterId}, total registered: {Emitters.Count}");
		}

		public static bool GetSignalForChannel(int channel)
		{
			var emitter = Emitters.FirstOrDefault(e => e.EmitChannel == channel);
			Debug.Log($"checking signal for channel {channel} returned {emitter != null : \"found\" : \"notfound\"} signal {emitter != null && emitter.Signal}");
			Debug.Log($"checking signal for channel {channel}. total registered: {Emitters.Count}");
			return emitter?.Signal ?? false;
		}

		public static void SetEmitterSignal(int emitterId, bool signal)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);

			if (emitter != null)
			{
				emitter.Signal = signal;
			}
		}
	}
}