using System.Text.RegularExpressions;
using KSerialization;

namespace NotificationTrigger
{
	public class NotificationTrigger : KMonoBehaviour
	{
		[MyCmpGet]
		private UserNameable userNameable;

		[Serialize]
		public bool WillPause;

		public StatusItem _statusItem;

		private static readonly EventSystem.IntraObjectHandler<NotificationTrigger> OnNameUpdatedDelegate = new EventSystem.IntraObjectHandler<NotificationTrigger>((component, data) => component.OnNameUpdated(data));
		private static readonly EventSystem.IntraObjectHandler<NotificationTrigger> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<NotificationTrigger>((component, data) => component.OnLogicValueChanged(data));

		private bool wasOn;

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
			//smi.StartSM();

			//if (userNameable.savedName.IsNullOrWhiteSpace())
			//{
			//	userNameable.SetName(NotificationTriggerConfig.DisplayName);
			//}


			Subscribe((int)GameHashes.NameChanged, OnNameUpdatedDelegate);
			Subscribe((int)GameHashes.LogicEvent, OnLogicValueChangedDelegate);

			var trimmed = userNameable.savedName.Trim();
			WillPause = trimmed.StartsWith("[P]");

			//userNameable.SetName(trimmed);
		}

		public void OnLogicValueChanged(object data)
		{
			var logicValueChanged = (LogicValueChanged) data;

			//if (logicValueChanged.portID != NotificationTriggerConfig.InputPortId)
		//		return;

			if (LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
			{
				if (this.wasOn)
					return;
			//	this.Toggle; ???
				this.wasOn = true;
				if (WillPause && !SpeedControlScreen.Instance.IsPaused)
					SpeedControlScreen.Instance.Pause(false);
				
				this.UpdateVisualState();
			}
			else
			{
				if (!this.wasOn)
					return;
				this.wasOn = false;
				this.UpdateVisualState();
			}
		}
		
		public void OnNameUpdated(object data)
		{
			var newName = (string) data;
			//var component = GetComponent<KSelectable>();

			//if (string.IsNullOrEmpty(newName))
			//{
			//	newName = NotificationTriggerConfig.DisplayName;
			//}

			var trimmed = newName.Trim();

			var regexp = new Regex(@"[^\[]+(?=\])");
			var match = regexp.Match(trimmed);
			var flags = match.ToString();

			WillPause = false;

			if (string.IsNullOrEmpty(flags))
			{
				_statusItem.notificationType = NotificationType.Good;
			}
			else switch (flags)
			{
				case "!":
					_statusItem.notificationType = NotificationType.Tutorial;
					break;
				case "!!":
					_statusItem.notificationType = NotificationType.Bad;
					break;
				case "!!!":
					_statusItem.notificationType = NotificationType.Bad;
					WillPause = true;
					break;
			}

			var notificationText = trimmed;

			if (!string.IsNullOrEmpty(flags))
			{
				notificationText = notificationText.Replace(flags, string.Empty);
			}

			notificationText = notificationText.Replace("[]", string.Empty);

			//NameDisplayScreen.Instance.UpdateName(gameObject);

			_statusItem.notificationText = notificationText.TrimStart();
		}

		private void UpdateVisualState()
		{
			GetComponent<KBatchedAnimController>().Play(wasOn ? "on" : "off");
		}
	}
}
