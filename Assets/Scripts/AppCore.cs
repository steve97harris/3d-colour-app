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
		private readonly Color highlightColour = Color.green;

		public Transform characterTransform;
		public List<Transform> customisableArmour;

		[SerializeField] private InputAction pressAction, screenPosAction, axisAction;
		[SerializeField] private float speed = 1;
		[SerializeField] private bool inverted;

		private Camera _mainCam;
		private Vector2 _rotation;
		private Vector3 _curScreenPos;
		private bool _isDragging;
		private bool _isArmourClickedOn => IsArmourClicked();
		
		private Material _activeMaterial;
		private Color _prevMaterialColour;

		private void Awake()
		{
			_mainCam = Camera.main;
			InitializeInputActions();
		}

		private void InitializeInputActions()
		{
			screenPosAction.Enable();
			pressAction.Enable();
			axisAction.Enable();
			screenPosAction.performed += context =>
			{
				_curScreenPos = context.ReadValue<Vector2>();
				HighlightCustomisableArmour();
			};
			axisAction.performed += context =>
			{
				_rotation = context.ReadValue<Vector2>();
			};
			pressAction.performed += context =>
			{
				if (_isArmourClickedOn)
					StartCoroutine(RotateCharacter());
			};
			pressAction.canceled += context =>
			{
				_isDragging = false;
			};
		}

		private bool IsArmourClicked()
		{
			var ray = _mainCam.ScreenPointToRay(_curScreenPos);
			return Physics.Raycast(ray, out var hit) && customisableArmour.Contains(hit.transform);
		}
		
		private IEnumerator RotateCharacter()
		{
			_isDragging = true;

			while(_isDragging)
			{
				_rotation *= speed;
				characterTransform.Rotate(Vector3.up * (inverted? 1: -1), _rotation.x, Space.World);
				yield return null;
			}
		}

		private void HighlightCustomisableArmour()
		{
			var ray = _mainCam.ScreenPointToRay(_curScreenPos);
			if (Physics.Raycast(ray, out var hit))
			{
				if (!customisableArmour.Contains(hit.transform))
					return;
				
				var meshRenderer = hit.transform.GetComponent<SkinnedMeshRenderer>();
				if (meshRenderer == null || meshRenderer.material == null)
					return;
				
				var selectedMaterial = meshRenderer.material;
				if (_activeMaterial == selectedMaterial)
					return;
				
				if (_prevMaterialColour != default)
					_activeMaterial.color = _prevMaterialColour;
				
				_activeMaterial = selectedMaterial;
				_prevMaterialColour = _activeMaterial.color;
				_activeMaterial.color = highlightColour;
				
				// Debug.Log($"{hit.transform.name.Split("_").Last()}");
			}
			else
			{
				if (_activeMaterial != null && _prevMaterialColour != default)
					_activeMaterial.color = _prevMaterialColour;
			}
		}
	}
}