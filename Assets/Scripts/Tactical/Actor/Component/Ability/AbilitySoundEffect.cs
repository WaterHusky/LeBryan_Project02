using UnityEngine;
using Tactical.Actor.Component;
using Tactical.Actor.Model;

namespace Tactical.Actor.Component {

	/// <summary>
	/// A simple component to play an ability sound effect.
	/// </summary>
	public class AbilitySoundEffect : MonoBehaviour {

		[SerializeField] public AudioClip hitEffect;
		[SerializeField] public AudioClip missEffect;

		private void OnEnable () {
			this.AddObserver(OnEffectHit, BaseAbilityEffect.HitNotification);
			this.AddObserver(OnEffectMiss, BaseAbilityEffect.MissNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnEffectHit, BaseAbilityEffect.HitNotification);
			this.RemoveObserver(OnEffectMiss, BaseAbilityEffect.MissNotification);
		}

		/// <summary>
		/// Plays an audio clip when the ability lands.
		/// </summary>
		///
		/// <param name="sender">The sender</param>
		/// <param name="args">The arguments</param>
		private void OnEffectHit (object sender, object args) {
			var info = args as HitInfo;
			if (info != null && info.audioSource && hitEffect) {
				Play(hitEffect, info.audioSource);
			}
		}

		/// <summary>
		/// Plays an audio clip when the ability misses.
		/// </summary>
		///
		/// <param name="sender">The sender</param>
		/// <param name="args">The arguments</param>
		private void OnEffectMiss (object sender, object args) {
			var info = args as HitInfo;
			if (info != null && info.audioSource && missEffect) {
				Play(missEffect, info.audioSource);
			}
		}

		/// <summary>
		/// Plays an audio clip.
		/// </summary>
		///
		/// <param name="clip">The clip to play.</param>
		/// <param name="audioSource">The audio source to play the clip from.</param>
		private void Play (AudioClip clip, AudioSource audioSource) {
			audioSource.clip = clip;
			audioSource.Play();
		}

	}

}
