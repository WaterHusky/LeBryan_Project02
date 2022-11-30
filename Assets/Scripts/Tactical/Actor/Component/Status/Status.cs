using UnityEngine;
using Tactical.Core;
using Tactical.UI.Component;

namespace Tactical.Actor.Component {

  public class Status : MonoBehaviour {

    public const string AddedNotification = "Status.AddedNotification";
    public const string RemovedNotification = "Status.RemovedNotification";

    private StatusMarkerController marker;

    public U Add<T, U> () where T : StatusEffect where U : StatusCondition {
      T effect = GetComponentInChildren<T>();
      if (effect == null) {
        effect = gameObject.AddChildComponent<T>();
        this.PostNotification(AddedNotification, effect);
      }

      // Add the status marker.
      marker = GetOrCreateIconController();
      marker.Add(effect.name);

      return effect.gameObject.AddChildComponent<U>();
    }

    public void Remove (StatusCondition target) {
      StatusEffect effect = target.GetComponentInParent<StatusEffect>();

      target.transform.SetParent(null);
      Destroy(target.gameObject);

      StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
      if (condition == null) {
        effect.transform.SetParent(null);
        Destroy(effect.gameObject);
        this.PostNotification(RemovedNotification, effect);
      }

      // Remove the status marker.
      marker.Remove(effect.name);
    }

    /// <summary>
    /// Gets or create the marker controller.
    /// </summary>
    ///
    /// <returns>The marker controller.</returns>
    private StatusMarkerController GetOrCreateIconController () {
      var controller = GetComponentInChildren<StatusMarkerController>();
      if (controller == null) {
        GameObject prefab = Resources.Load<GameObject>("UI/World Space/Status Markers/Status Marker Controller");
        if (!prefab) {
        	Debug.LogError("Missing prefab for Status Marker.");
        	return null;
        }
        var instance = Instantiate(prefab);
        instance.name = "Status Markers";
        instance.transform.SetParent(transform, false);
        instance.transform.localPosition = new Vector3(0f, 1f, 0f);
        controller = instance.GetComponent<StatusMarkerController>();
      }

      return controller;
    }

  }

}
