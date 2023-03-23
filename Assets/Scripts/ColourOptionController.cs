using System;
using UnityEngine;
namespace DefaultNamespace
{
	public class ColourOptionController : MonoBehaviour
	{
		private Ray ray;

		private void Update()
		{
			if (Camera.main == null)
				return;
			
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out var hit))
			{
				Debug.LogError(hit.collider.name);
			}
		}
	}
}