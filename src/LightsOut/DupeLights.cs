using UnityEngine;

namespace LightsOut
{
	public class DupeLights : KMonoBehaviour, ISim1000ms
	{
		public Light2D SuitLamp { get; set; }
		public Light2D DupeLight { get; set; }

		[MyCmpGet]
		private MinionIdentity _minionIdentity;

		[MyCmpGet]
		private SuitEquipper _suitEquipper;

		protected override void OnPrefabInit()
		{
			SuitLamp = _minionIdentity.gameObject.AddComponent<Light2D>();
			SuitLamp.Color = Color.yellow;
			SuitLamp.Offset = new Vector2(0f, 1f);
			SuitLamp.Range = 10;
			SuitLamp.Lux = LightsOutMod.ConfigManager.Config.SuitLightLux;
			SuitLamp.shape = LightShape.Circle;
			SuitLamp.enabled = false;

			DupeLight = _minionIdentity.gameObject.AddComponent<Light2D>();
			DupeLight.Color = Color.white;
			DupeLight.Offset = new Vector2(0f, 1f);
			DupeLight.Range = 3;
			DupeLight.Lux = LightsOutMod.ConfigManager.Config.DupeLightLux;
			DupeLight.shape = LightShape.Circle;
			DupeLight.enabled = LightsOutMod.ConfigManager.Config.DupeLight;
		}

		public void Sim1000ms(float dt)
		{
			SuitLamp.enabled = LightsOutMod.ConfigManager.Config.SuitLight && _suitEquipper.IsWearingAirtightSuit();
		}
	}
}