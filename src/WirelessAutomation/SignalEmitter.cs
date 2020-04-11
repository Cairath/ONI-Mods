namespace WirelessAutomation
{
	public class SignalEmitter
	{
		public int Id { get; set; }
		public int EmitChannel { get; set; }
		public int Signal { get; set; }

		public SignalEmitter(int emitChannel, int signal)
		{
			Signal = signal;
			EmitChannel = emitChannel;
		}
	}
}
