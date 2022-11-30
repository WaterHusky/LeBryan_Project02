using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Tactical.Core.Enums;
using Tactical.Actor.Component;

namespace Tactical.UI.Component {

	public class StatPanel : MonoBehaviour {

		public Panel panel;
		public Sprite allyBackground;
		public Sprite enemyBackground;
		public Image background;
		public Image avatar;
		public Text nameLabel;
		public Text lvLabel;
		public Text expLabel;
		public Text hpLabel;
		public Slider hpSlider;
		public Text mpLabel;
		public Slider mpSlider;
		public Text ctLabel;
		public Slider ctSlider;

		public void Start () {
			Assert.IsNotNull(panel, "panel required");
			Assert.IsNotNull(allyBackground, "allyBackground required");
			Assert.IsNotNull(enemyBackground, "enemyBackground required");
			Assert.IsNotNull(background, "background required");
			Assert.IsNotNull(avatar, "avatar required");
			Assert.IsNotNull(nameLabel, "nameLabel required");
			Assert.IsNotNull(lvLabel, "lvLabel required");
			Assert.IsNotNull(expLabel, "expLabel required");
			Assert.IsNotNull(hpLabel, "hpLabel required");
			Assert.IsNotNull(hpSlider, "hpSlider required");
			Assert.IsNotNull(mpLabel, "mpLabel required");
			Assert.IsNotNull(mpSlider, "mpSlider required");
			Assert.IsNotNull(ctLabel, "ctLabel required");
			Assert.IsNotNull(ctSlider, "ctSlider required");
		}

		public void Display (GameObject obj) {
			Alliance alliance = obj.GetComponent<Alliance>();
			background.sprite = alliance.type == Alliances.Enemy ? enemyBackground : allyBackground;

			// TODO: Need a component which provides this data.
			// avatar.sprite = null;

			nameLabel.text = obj.name;
			var stats = obj.GetComponent<Stats>();
			if (stats) {
				lvLabel.text = string.Format("Lv. {0}", stats[StatTypes.LVL]);
				expLabel.text = string.Format("Exp. {0}", stats[StatTypes.EXP]);

				hpLabel.text = string.Format("HP {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP]);
				hpSlider.value = (float) stats[StatTypes.HP] / stats[StatTypes.MHP];

				mpLabel.text = string.Format("MP {0} / {1}", stats[StatTypes.MP], stats[StatTypes.MMP]);
				mpSlider.value = (float) stats[StatTypes.MP] / stats[StatTypes.MMP];

				ctLabel.text = string.Format("CT {0} / {1}", Mathf.Min(stats[StatTypes.CTR], 100), 100);
				ctSlider.value = (float) Mathf.Min(stats[StatTypes.CTR], 100) / 100;
			}
		}

	}

}
