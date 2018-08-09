using ProcGen;

namespace DrywallHidesPipes
{
	public class ZoneTileClone : KMonoBehaviour
	{
		public int width = 1;
		public int height = 1;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			int cell = Grid.PosToCell((KMonoBehaviour) this);
			for (int x = 0; x < this.width; ++x)
			{
				for (int y = 0; y < this.height; ++y)
					SimMessages.ModifyCellWorldZone(Grid.OffsetCell(cell, x, y), (byte) 0);
			}
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			int cell1 = Grid.PosToCell((KMonoBehaviour) this);
			for (int x = 0; x < this.width; ++x)
			{
				for (int y = 0; y < this.height; ++y)
				{
					int cell2 = Grid.OffsetCell(cell1, x, y);
					SubWorld.ZoneType subWorldZoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(cell2);
					byte zone_id = subWorldZoneType != SubWorld.ZoneType.Space ? (byte) subWorldZoneType : byte.MaxValue;
					SimMessages.ModifyCellWorldZone(cell2, zone_id);
				}
			}
		}
	}
}