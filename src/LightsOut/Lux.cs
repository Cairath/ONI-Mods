namespace LightsOut
{
	public static class Lux
	{
		/// <summary>
		/// Numerically computes the average lux from valid neighbors around a target cell. If there are no valid neighbors, returns zero.
		/// </summary>
		/// <returns>
		/// The average lux from valid neighbors around the target cell.
		/// </returns>
		public static int NeighborAverage(int targetCell)
		{
			var count = 0;
			var average = 0d;
			// Aggregate lux from valid neighbor cells.
			AggregateLux(Grid.CellAbove(targetCell), ref count, ref average);
			AggregateLux(Grid.CellUpRight(targetCell), ref count, ref average);
			AggregateLux(Grid.CellRight(targetCell), ref count, ref average);
			AggregateLux(Grid.CellDownRight(targetCell), ref count, ref average);
			AggregateLux(Grid.CellBelow(targetCell), ref count, ref average);
			AggregateLux(Grid.CellDownLeft(targetCell), ref count, ref average);
			AggregateLux(Grid.CellLeft(targetCell), ref count, ref average);
			AggregateLux(Grid.CellUpLeft(targetCell), ref count, ref average);
			// Truncate the fractional part because lux is measured as an integer.
			return (int)average;
		}

		/// <summary>
		/// Aggregates lux from a valid cell into a count and an average.
		/// </summary>
		/// <param name="cell">The cell to sample for lux.</param>
		/// <param name="count">The number of lux samples collected. Incremented if the cell is valid.</param>
		/// <param name="average">The average lux from samples collected. Updated if the cell is valid.</param>
		private static void AggregateLux(int cell, ref int count, ref double average)
		{
			if (Grid.IsValidCell(cell))
			{
				count++;
				// Incrementally update the average to avoid numerical overflows.
				average += (Grid.LightIntensity[cell] - average) / count;
			}
		}
	}
}
