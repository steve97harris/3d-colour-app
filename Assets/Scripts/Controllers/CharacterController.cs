using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DefaultNamespace
{
	public class CharacterController : MonoBehaviour
	{
		public Transform characterTransform;
		public List<Transform> customisableArmour;

		[HideInInspector] public Vector2 rotation;
		[HideInInspector] public bool isDragging;

		[SerializeField] private float speed = 1;
		[SerializeField] private bool inverted;

		public IEnumerator RotateCharacter()
		{
			isDragging = true;

			while(isDragging)
			{
				rotation *= speed;
				characterTransform.Rotate(Vector3.up * (inverted? 1: -1), rotation.x, Space.World);
				yield return null;
			}
		}
		
		
	}
}