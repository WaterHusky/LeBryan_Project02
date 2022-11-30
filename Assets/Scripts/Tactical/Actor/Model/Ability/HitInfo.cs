using UnityEngine;
using Tactical.Grid.Component;

namespace Tactical.Actor.Model {

	public class HitInfo {

		public Tile tile;
		public int damage;
		public AudioSource audioSource;

		public HitInfo (Tile tile, int damage) {
			this.tile = tile;
			this.damage = damage;
		}

		public HitInfo (Tile tile, int damage, AudioSource audioSource) {
			this.tile = tile;
			this.damage = damage;
			this.audioSource = audioSource;
		}

	}

}
