using System.Text.RegularExpressions;
using KSerialization;

namespace NotificationTrigger
{
	public class NotificationTrigger : StateMachineComponent<NotificationTrigger.SMInstance>
	{
		[Serialize]
		public string Name = string.Empty;

		[Serialize]
		public bool WillPause;

		public StatusItem _statusItem;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();

			_statusItem
				= new StatusItem(NotificationTriggerConfig.Id,
					"MISC",
					string.Empty,
					StatusItem.IconType.Info,
					NotificationType.Bad,
					false,
					OverlayModes.None.ID);

			_statusItem.AddNotification(null, "Notification Trigger");
		}

		protected override void OnSpawn()
		{
			smi.StartSM();

			if (Name.IsNullOrWhiteSpace())
			{
				Name = NotificationTriggerConfig.DisplayName;
			}

			var trimmed = Name.Trim();
			WillPause = trimmed.StartsWith("[P]");

			SetName(trimmed);
		}

		public void SetName(string name)
		{
			var component = GetComponent<KSelectable>();

			if (string.IsNullOrEmpty(name))
			{
				name = NotificationTriggerConfig.DisplayName;
			}

			var trimmed = name.Trim();

			var regexp = new Regex(@"[^\[]+(?=\])");
			var match = regexp.Match(trimmed);
			var flags = match.ToString();

			WillPause = false;

			if (string.IsNullOrEmpty(flags))
			{
				_statusItem.notificationType = NotificationType.Good;
			}
			else if (flags == "!")
			{
				_statusItem.notificationType = NotificationType.Tutorial;
			}
			else if (flags == "!!")
			{
				_statusItem.notificationType = NotificationType.Bad;
			}
			else if (flags == "!!!")
			{
				_statusItem.notificationType = NotificationType.Bad;
				WillPause = true;
			}

			var notificationText = trimmed;

			if (!string.IsNullOrEmpty(flags))
			{
				notificationText = notificationText.Replace(flags, string.Empty);
			}

			notificationText = notificationText.Replace("[]", string.Empty);

			this.name = trimmed;
			Name = trimmed;
			
			if (component != null)
			{
				component.SetName(name);
			}

			gameObject.name = name;
			NameDisplayScreen.Instance.UpdateName(gameObject);

			_statusItem.notificationText = notificationText.TrimStart();
		}

		public class SMInstance : GameStateMachine<States, SMInstance, NotificationTrigger, object>.GameInstance
		{
			public SMInstance(NotificationTrigger master) : base(master) { }
		}

		public class States : GameStateMachine<States, SMInstance, NotificationTrigger>
		{
			public State Off;
			public State On;

			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = Off;

				Off
					.PlayAnim("off")
					.EventTransition(GameHashes.OperationalChanged, On, smi => smi.GetComponent<Operational>().IsOperational);
				On
					.PlayAnim("on")
					.Enter(smi =>
					{
						smi.GetComponent<Operational>().SetActive(true);
						if (smi.master.WillPause)
						{
							SpeedControlScreen.Instance.Pause();
						}
					})
					.ToggleStatusItem(smi => smi.master._statusItem)
					.EventTransition(GameHashes.OperationalChanged, Off, smi => !smi.GetComponent<Operational>().IsOperational);
			}
		}
	}
}
