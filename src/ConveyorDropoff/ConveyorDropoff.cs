namespace ConveyorDropoff
{
	public class ConveyorDropoff : KMonoBehaviour, ISim1000ms
	{
		[MyCmpGet]
		private Storage storage;

		public void Sim1000ms(float dt)
		{
			storage.DropAll();
		}
	}
}