using UnityEngine;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;

namespace Tactical.Actor.Component {

	public class Stats : MonoBehaviour {

		// Indexer to allow us to iterate on the stats like an array.
		public int this[StatTypes s] {
			get { return _data[(int) s]; }
			set { SetValue(s, value, true); }
		}

		[SerializeField] private int[] _data = new int[ (int) StatTypes.Count ];
		private static Dictionary<StatTypes, string> _willChangeNotifications = new Dictionary<StatTypes, string>();
		private static Dictionary<StatTypes, string> _didChangeNotifications = new Dictionary<StatTypes, string>();

		public void SetValue (StatTypes type, int value, bool allowExceptions) {
			int oldValue = this[type];
			if (oldValue == value) {
				return;
			}

			if (allowExceptions) {
				// Allow exceptions to the rule here
				var exc = new ValueChangeException(oldValue, value);

				// The notification is unique per stat type
				this.PostNotification(WillChangeNotification(type), exc);

				// Did anything modify the value?
				value = Mathf.FloorToInt(exc.GetModifiedValue());

				// Did something nullify the change?
				if (!exc.toggle || value == oldValue) {
					return;
				}
			}

			_data[(int)type] = value;
			this.PostNotification(DidChangeNotification(type), oldValue);
		}

		public static string WillChangeNotification (StatTypes type) {
			if (!_willChangeNotifications.ContainsKey(type)) {
				_willChangeNotifications.Add(type, string.Format("Stats.{0}WillChange", type));
			}
			return _willChangeNotifications[type];
		}

		public static string DidChangeNotification (StatTypes type) {
			if (!_didChangeNotifications.ContainsKey(type)) {
				_didChangeNotifications.Add(type, string.Format("Stats.{0}DidChange", type));
			}
			return _didChangeNotifications[type];
		}

	}

}
