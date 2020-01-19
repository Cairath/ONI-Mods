using UnityEngine;

namespace Wallpaper
{
	public class ColorRefresher : MonoBehaviour, ISim1000ms
	{
		private bool _dirty;

		public void MarkDirty()
		{
			_dirty = true;
		}

		public void Sim1000ms(float dt)
		{
			if (_dirty)
			{
				ColorTools.RecolorWalls();
				_dirty = false;
			}
		}
	}
}