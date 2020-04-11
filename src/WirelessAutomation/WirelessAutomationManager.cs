using System.Collections.Generic;
using System.Linq;
using KSerialization;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public static class WirelessAutomationManager
	{
		public static int WirelessLogicEvent = (int)GameHashes.LogicEvent + 1;

		public static string SliderTooltipKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TOOLTIP";
		public static string SliderTooltip = "Select channel to tune in the device";

		public static string SliderTitleKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TITLE";
		public static string SliderTitle = "Channel";

		private static List<SignalEmitter> Emitters { get; } = new List<SignalEmitter>();
		private static List<SignalReceiver> Receivers { get; } = new List<SignalReceiver>();

		public static void ResetEmittersList()
		{
			Emitters.Clear();
		}

		public static void ResetReceiversList()
		{
			Receivers.Clear();
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

		public static int RegisterReceiver(SignalReceiver receiver)
		{
			var newId = 0;
			if (Receivers.Count > 0)
			{
				newId = Receivers.Max(e => e.Id) + 1;
			}

			receiver.Id = newId;
			Receivers.Add(receiver);

			return receiver.Id;
		}

		public static void UnregisterEmitter(int emitterId)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);
			Emitters.Remove(emitter);
		}

		public static void UnregisterReceiver(int id)
		{
			var receiver = Receivers.FirstOrDefault(e => e.Id == id);
			Receivers.Remove(receiver);
		}

		public static void SetEmitterSignal(int emitterId, int signal)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);

			if (emitter == null) return;

			emitter.Signal = signal;
			NotifyReceivers(emitter.EmitChannel, signal);
		}

		public static void ChangeEmitterChannel(int emitterId, int channel)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Id == emitterId);

			if (emitter == null) return;

			var othersOnChannel =
				Emitters.Where(e => e.Id != emitterId && e.EmitChannel == emitter.EmitChannel).ToList();

			var leaveSignal = othersOnChannel.Any() ? othersOnChannel.First().Signal : 0;
			NotifyReceivers(emitter.EmitChannel, leaveSignal);

			emitter.EmitChannel = channel;
			NotifyReceivers(emitter.EmitChannel, emitter.Signal);
		}

		public static void ChangeReceiverChannel(int receiverId, int channel)
		{
			var receiver = Receivers.FirstOrDefault(r => r.Id == receiverId);

			if (receiver == null) return;

			var emittersOnChannel =
				Emitters.Where(e => e.EmitChannel == channel).ToList();

			var signal = emittersOnChannel.Any() ? emittersOnChannel.First().Signal : 0;
			NotifyReceivers(channel, signal);

			receiver.Channel = channel;
		}

		private static void NotifyReceivers(int channel, int signal)
		{
			foreach (var receiver in Receivers)
			{
				receiver.GameObject?.Trigger(WirelessLogicEvent, new WirelessLogicValueChanged
				{
					Signal = signal,
					Channel = channel
				});
			}
		}
	}
}