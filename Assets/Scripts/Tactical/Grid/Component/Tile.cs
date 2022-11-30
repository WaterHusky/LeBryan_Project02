using UnityEngine;
using Tactical.Grid.Model;

namespace Tactical.Grid.Component {

	[ExecuteInEditMode]
	public class Tile : MonoBehaviour {

		public const float STEP_HEIGHT = 0.25f;
		public Point pos;
		public int height;
		public int width = 1;
		public Vector3 center {
			get { return new Vector3(pos.x, height * STEP_HEIGHT, pos.y); }
		}
		public GameObject content;
		[HideInInspector] public GameObject overlay;
		[HideInInspector] public Tile prev;
		[HideInInspector] public int distance;

		public void Grow () {
			height++;
			Match();
		}

		public void Shrink () {
			height--;
			Match ();
		}

		public void Load (Point p, int h) {
			pos = p;
			height = h;
			Match();
		}

		public void Load (Vector3 v) {
			Load(new Point((int)v.x, (int)v.z), (int)v.y);
		}

		private void Update () {
			Match();
		}

		private void Match () {
			transform.localPosition = new Vector3(pos.x, height * STEP_HEIGHT / 2f, pos.y );
			transform.localScale = new Vector3(width, height * STEP_HEIGHT, width);
		}
	}

}
