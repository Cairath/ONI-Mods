namespace WirelessAutomation
{
	public class SignalEmitter
	{
		public int Id { get; set; }
		public int EmitChannel { get; set; }
		public bool Signal { get; set; }

		public SignalEmitter(int emitChannel, bool signal)
		{
			Signal = signal;
			EmitChannel = emitChannel;
		}
	}
}
