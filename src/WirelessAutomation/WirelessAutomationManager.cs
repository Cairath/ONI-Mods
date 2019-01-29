using System.Collections.Generic;
using System.Linq;
using KSerialization;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public static class WirelessAutomationManager
	{
		public static string SliderTooltipKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TOOLTIP";
		public static string SliderTooltip = "Select channel to tune in the device";

		public static string SliderTitleKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TITLE";
		public static string SliderTitle = "Channel";

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

			return emitter.Id;
		}

		public static void UnregisterEmitter(int emitterId)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);
			Emitters.Remove(emitter);
		}

		public static bool GetSignalForChannel(int channel)
		{
			var emitter = Emitters.FirstOrDefault(e => e.EmitChannel == channel);
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

		public static void ChangeEmitterChannel(int emitterId, int channel)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);

			if (emitter != null)
			{
				emitter.EmitChannel = channel;
			}
		}
	}
}