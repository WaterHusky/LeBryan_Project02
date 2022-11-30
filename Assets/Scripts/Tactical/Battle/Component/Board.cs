using UnityEngine;
using System;
using System.Collections.Generic;
using Tactical.Grid.Model;
using Tactical.Grid.Component;

namespace Tactical.Battle.Component {

	public class Board : MonoBehaviour {

		public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
		public Point min {
			get { return _min; }
		}
		public Point max {
			get { return _max; }
		}

		[Header(" - Required - ")]
		public Transform tilesContainer;
		public Transform unitsContainer;
		[SerializeField] private GameObject tileOverlayPrefab;
		[SerializeField] private GameObject tilePrefab;
		private Point[] dirs = new Point[4] {
			new Point(0, 1),
			new Point(0, -1),
			new Point(1, 0),
			new Point(-1, 0)
		};
		private Color selectedTileColor = new Color(0.15f, 0.15f, 0.4f, 1);
		private Point _min;
		private Point _max;

		public void Load (LevelData data) {
			_min = new Point(int.MaxValue, int.MaxValue);
			_max = new Point(int.MinValue, int.MinValue);

			for (int i = 0; i < data.tiles.Count; ++i) {
				var instance = Instantiate(tilePrefab) as GameObject;
				instance.transform.parent = tilesContainer;
				Tile t = instance.GetComponent<Tile>();
				if (t == null) {
					throw new Exception("Trying to instanciate an invalid tile: " + instance.name);
				}
				t.Load(data.tiles[i]);
				tiles.Add(t.pos, t);

				_min.x = Mathf.Min(_min.x, t.pos.x);
				_min.y = Mathf.Min(_min.y, t.pos.y);
				_max.x = Mathf.Max(_max.x, t.pos.x);
				_max.y = Mathf.Max(_max.y, t.pos.y);
			}
		}

		public void SelectTiles (List<Tile> tiles) {
			for (int i = tiles.Count - 1; i >= 0; --i) {
				GameObject instance = Instantiate(tileOverlayPrefab);
				instance.transform.parent = tiles[i].transform;
				instance.transform.localPosition = Vector3.up * 0.5f;
				tiles[i].overlay = instance;
			}
		}

		public void DeSelectTiles (List<Tile> tiles) {
			for (int i = tiles.Count - 1; i >= 0; --i) {
				Destroy(tiles[i].overlay);
				tiles[i].overlay = null;
			}
		}

		public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile) {
			var retValue = new List<Tile>();
			retValue.Add(start);

			ClearSearch();
			var checkNext = new Queue<Tile>();
			var checkNow = new Queue<Tile>();

			start.distance = 0;
			checkNow.Enqueue(start);

			while (checkNow.Count > 0) {
				Tile t = checkNow.Dequeue();

				for (int i = 0; i < 4; ++i) {
					Tile next = GetTile(t.pos + dirs[i]);
					if (next == null || next.distance <= t.distance + 1) {
						continue;
					}

					if (addTile(t, next)) {
						next.distance = t.distance + 1;
						next.prev = t;
						checkNext.Enqueue(next);
						retValue.Add(next);
					}
				}

				if (checkNow.Count == 0) {
					SwapReference(ref checkNow, ref checkNext);
				}
			}

			return retValue;
		}

		public Tile GetTile (Point p) {
			return tiles.ContainsKey(p) ? tiles[p] : null;
		}

		private void SwapReference (ref Queue<Tile> a, ref Queue<Tile> b) {
			Queue<Tile> temp = a;
			a = b;
			b = temp;
		}

		private void ClearSearch () {
			foreach (Tile t in tiles.Values) {
				t.prev = null;
				t.distance = int.MaxValue;
			}
		}
	}

}
