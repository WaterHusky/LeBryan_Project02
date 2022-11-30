/*
The MIT License (MIT)

Copyright (c) 2016 Jonathan Parham

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using System.Collections.Generic;

public class GameObjectPoolController : MonoBehaviour {

	private static GameObjectPoolController Instance {
		get {
			if (instance == null) {
				CreateSharedInstance();
			}
			return instance;
		}
	}
	private static GameObjectPoolController instance;

	private static Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();

	private void Awake () {
		if (instance != null && instance != this) {
			Destroy(this);
		} else {
			instance = this;
		}
	}

	public static void SetMaxCount (string key, int maxCount) {
		if (!pools.ContainsKey(key)) {
			return;
		}
		PoolData data = pools[key];
		data.maxCount = maxCount;
	}

	public static bool AddEntry (string key, GameObject prefab, int prepopulate, int maxCount) {
		if (pools.ContainsKey(key)) {
			return false;
		}

		var data = new PoolData();
		data.prefab = prefab;
		data.maxCount = maxCount;
		data.pool = new Queue<Poolable>(prepopulate);
		pools.Add(key, data);

		for (int i = 0; i < prepopulate; ++i) {
			Enqueue( CreateInstance(key, prefab) );
		}

		return true;
	}

	public static void ClearEntry (string key) {
		if (!pools.ContainsKey(key)) {
			return;
		}

		PoolData data = pools[key];
		while (data.pool.Count > 0) {
			Poolable obj = data.pool.Dequeue();
			Destroy(obj.gameObject);
		}
		pools.Remove(key);
	}

	public static void Enqueue (Poolable sender) {
		if (sender == null || sender.isPooled || !pools.ContainsKey(sender.key)) {
			return;
		}

		PoolData data = pools[sender.key];
		if (data.pool.Count >= data.maxCount) {
			Destroy(sender.gameObject);
			return;
		}

		data.pool.Enqueue(sender);
		sender.isPooled = true;
		sender.transform.SetParent(Instance.transform);
		sender.gameObject.SetActive(false);
	}

	public static Poolable Dequeue (string key) {
		if (!pools.ContainsKey(key)) {
			return null;
		}

		PoolData data = pools[key];
		if (data.pool.Count == 0) {
			return CreateInstance(key, data.prefab);
		}

		Poolable obj = data.pool.Dequeue();
		obj.isPooled = false;
		return obj;
	}

	private static void CreateSharedInstance () {
		var obj = new GameObject("GameObject Pool Controller");
		DontDestroyOnLoad(obj);
		instance = obj.AddComponent<GameObjectPoolController>();
	}

	private static Poolable CreateInstance (string key, GameObject prefab) {
		var newInstance = Instantiate(prefab);
		Poolable p = newInstance.AddComponent<Poolable>();
		p.key = key;
		return p;
	}

}
