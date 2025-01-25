using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public class AutoDestroy : MonoBehaviour
	{
		private void Start()
		{
			var audio = GetComponent<AudioSource>();

			if (audio.clip)
			{
				Initialize(audio.clip.length + 1.0f);
				return;
			}


			Initialize(5.0f);
		}
		public void Initialize(float timeTillSelfDestroy)
		{
			Invoke(nameof(SelfDestroy), timeTillSelfDestroy);
		}

		private void SelfDestroy()
		{
			if (this != null && gameObject != null)
			{
				Destroy(gameObject);
			}
		}
	}
}
