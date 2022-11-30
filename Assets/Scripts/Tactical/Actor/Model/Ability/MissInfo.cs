using UnityEngine;
using Tactical.Grid.Component;

namespace Tactical.Actor.Model {

	public class MissInfo {

		public Tile tile;
		public AudioSource audioSource;

		public MissInfo (Tile tile, AudioSource audioSource) {
			this.tile = tile;
			this.audioSource = audioSource;
		}

	}

}
