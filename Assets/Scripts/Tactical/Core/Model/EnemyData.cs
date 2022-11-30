using UnityEngine;
using System;

namespace Tactical.Core.Model {

	public class EnemyData : ScriptableObject {

		public int hp;
		public int hitCount;
		public int damage;
		public int agility;
		public int xp;
		public int gold;

		public void Load (string line) {
			string[] elements = line.Split(',');
			name = elements[0];
			hp = Convert.ToInt32( elements[1] );
			hitCount = Convert.ToInt32( elements[2] );
			damage = Convert.ToInt32( elements[3] );
			agility = Convert.ToInt32( elements[4] );
			xp = Convert.ToInt32( elements[5] );
			gold = Convert.ToInt32( elements[6] );
		}

	}

}
