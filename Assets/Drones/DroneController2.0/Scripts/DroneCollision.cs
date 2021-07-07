using UnityEngine;
using System.Collections;

public class DroneCollision : MonoBehaviour {

	

    #region PUBLIC VARIABLES

    [Tooltip("Sparks GameObject prefab that will be created when crashing the drone.")]
    public GameObject sparks;

    #endregion

    #region Mono Behaviour METHODS

    void Awake()
    {
        if (!sparks)
        {
            print("Missing sparks particle prefab!");
        }
    }

	void OnCollisionStay(Collision other)
    {
		//Create sparks only if proppelers hit object
		if (other.contacts[0].thisCollider.GetComponent<ProppelerCollisionScript>())
		{
			other.contacts[0].thisCollider.GetComponent<ProppelerCollisionScript>().ProppelerHitObject();
			SparkCreator(other);
		}
    }

	#endregion

	#region CUSTOM METHODS

	/// <summary>
	/// Creates sparks at point of hit.
	/// </summary>
	void SparkCreator(Collision other)
	{
		if (other.transform)
		{
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal) * Quaternion.Euler(-90, 0, 0);
			Vector3 pos = contact.point;

			if (sparks)
			{
				GameObject spark = (GameObject)Instantiate(sparks, pos, rot);
				spark.transform.localScale = transform.localScale * 2;
				foreach (Transform _spark in spark.transform)
				{
					_spark.localScale = transform.localScale * 2;
				}
				StartCoroutine(SparksCleaner(spark));
			}
			else
			{
				Debug.LogError("You did not assign a spark prefab effect, default is located in DroneController/Prefabs/...");
			}

		}
	}

	#endregion

	#region PRIVATE Coroutine METHODS

	IEnumerator SparksCleaner(GameObject _spark)
    {
        yield return new WaitForSeconds(1);
        Destroy(_spark);
    }

    #endregion

}
