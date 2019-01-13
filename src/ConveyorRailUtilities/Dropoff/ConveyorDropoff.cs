namespace ConveyorRailUtilities.Dropoff
{
	public class ConveyorDropoff : KMonoBehaviour, ISim1000ms
	{
		[MyCmpGet]
		private Storage _storage;

		public void Sim1000ms(float dt)
		{
			_storage.DropAll();
		}
	}
}
