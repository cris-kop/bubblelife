using UnityEngine;

public class Pickup : MonoBehaviour
{
	private void Awake()
	{
		var pickupMovers = GetComponents<SimpleMover_Base>();
		var activeIndex = Random.Range(0, pickupMovers.Length);

		for(int i = 0; i < pickupMovers.Length; i++)
		{
			pickupMovers[i].enabled = i == activeIndex;
		}
	}
}
