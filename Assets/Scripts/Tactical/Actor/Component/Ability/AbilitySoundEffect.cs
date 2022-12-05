using UnityEngine;
using Tactical.Actor.Component;
using Tactical.Actor.Model;
using Tactical.Battle.Controller;

namespace Tactical.Actor.Component {

	/// <summary>
	/// A simple component to play an ability sound effect.
	/// </summary>
	public class AbilitySoundEffect : MonoBehaviour {

		[SerializeField] public AudioClip hitEffect;
		[SerializeField] public AudioClip missEffect;
		BattleController battleController;

        void Awake()
        {
			battleController = FindObjectOfType<BattleController>();
		}

		/// <summary>
		/// Plays an audio clip when the ability lands.
		/// </summary>
		///
		/// <param name="sender">The sender</param>
		/// <param name="args">The arguments</param>
		public void OnEffectHit (object sender, object args) {
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
		public void OnEffectMiss (object sender, object args) {
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
			Debug.Log(clip.name);
			audioSource.clip = clip;
			audioSource.Play();
		}

	}

}
