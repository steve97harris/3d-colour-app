using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace
{
	public class AppCore : MonoBehaviour
	{
		public static AppCore Instance;
		
		public Camera mainCam;
		public ColorPicker colourPicker;
		public ColourPickerController colourPickerController;
		public InputController inputController;
		public CharacterController characterController;

		private void Awake()
		{
			Instance = this;
			inputController.InitializeInputActions();
		}
	}
}