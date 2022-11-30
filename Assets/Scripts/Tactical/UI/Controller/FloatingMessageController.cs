using UnityEngine;
using Tactical.UI.Component;
using Tactical.Grid.Component;
using Tactical.Actor.Component;
using Tactical.Actor.Model;

namespace Tactical.UI.Controller {

	public class FloatingMessageController : MonoBehaviour {

		[SerializeField] private GameObject messagePrefab;

		private void OnEnable () {
			this.AddObserver(OnEffectHit, BaseAbilityEffect.HitNotification);
			this.AddObserver(OnEffectMiss, BaseAbilityEffect.MissNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnEffectHit, BaseAbilityEffect.HitNotification);
			this.RemoveObserver(OnEffectMiss, BaseAbilityEffect.MissNotification);
		}

		/// <summary>
		/// Creates a floating message with the damage / heal values.
		/// </summary>
		///
		/// <param name="sender">The sender</param>
		/// <param name="args">The arguments</param>
		private void OnEffectHit (object sender, object args) {
			var info = args as HitInfo;
			if (info != null) {
				CreateFloatingMessage(info.tile, info.damage.ToString());
			}
		}

		/// <summary>
		/// Creates a floating message "missed".
		/// </summary>
		///
		/// <param name="sender">The sender</param>
		/// <param name="args">The arguments</param>
		private void OnEffectMiss (object sender, object args) {
			var info = args as MissInfo;
			if (info != null) {
				CreateFloatingMessage(info.tile, "Miss");
			}
		}

		/// <summary>
		/// Creates a floating message.
		/// </summary>
		///
		/// <param name="tile">The tile to create the message at.</param>
		/// <param name="message">The message of the message.</param>
		private void CreateFloatingMessage (Tile tile, string message) {
			if (messagePrefab == null) {
				Debug.LogError("No message prefab.");
				return;
			}

			GameObject instance = Instantiate(messagePrefab);
			instance.transform.position = tile.transform.position;
			var floatingMessage = instance.GetComponent<FloatingMessage>();
			floatingMessage.message = message;
			floatingMessage.duration = 0.5f;
		}

	}

}
