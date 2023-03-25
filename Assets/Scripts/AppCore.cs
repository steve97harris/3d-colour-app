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
		public ColorPicker colourPicker;
		public Transform characterTransform;
		public List<Transform> customisableArmour;

		[SerializeField] private InputActionReference _pressAction, _holdAction, _screenPosAction, _axisAction;

		[SerializeField] private float speed = 1;
		[SerializeField] private bool inverted;

		private Camera _mainCam;
		private Vector2 _rotation;
		private Vector3 _curScreenPos;
		private bool _isDragging;
		private bool _isArmourHit => IsArmourHit();

		private Transform _armourHit;
		private Material _selectedMaterial;
		private Material _highlightedMaterial;

		private void Awake()
		{
			_mainCam = Camera.main;
			InitializeInputActions();
		}

		private void InitializeInputActions()
		{
			_pressAction.action.Enable();
			_screenPosAction.action.Enable();
			_axisAction.action.Enable();
			_holdAction.action.Enable();

			_screenPosAction.action.performed += context =>
			{
				_curScreenPos = context.ReadValue<Vector2>();
				HighlightCustomisableArmour();
			};
			_axisAction.action.performed += context =>
			{
				_rotation = context.ReadValue<Vector2>();
			};

			_pressAction.action.performed += context =>
			{
				if (!_isArmourHit)
					return;
				
				var meshRenderer = _armourHit.transform.GetComponent<SkinnedMeshRenderer>();
				if (meshRenderer == null || meshRenderer.material == null)
					return;
				
				_selectedMaterial = meshRenderer.material;
				OpenColourPicker();
			};
			
			_holdAction.action.performed += context =>
			{
				if (_isArmourHit)
					StartCoroutine(RotateCharacter());
			};
			_holdAction.action.canceled += context =>
			{
				_isDragging = false;
			};
		}

		private bool IsArmourHit()
		{
			var ray = _mainCam.ScreenPointToRay(_curScreenPos);
			var isArmourHit = Physics.Raycast(ray, out var hit) && customisableArmour.Contains(hit.transform);
			if (isArmourHit)
				_armourHit = hit.transform;
			return isArmourHit;
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
			if (_isArmourHit)
			{
				for (int i = 0; i < customisableArmour.Count; i++)
				{
					var armourTransform = customisableArmour[i];
					var meshRenderer = armourTransform.GetComponent<SkinnedMeshRenderer>();
					if (meshRenderer == null || meshRenderer.material == null)
						continue;
					SetMaterialHighlighted(armourTransform == _armourHit, meshRenderer.material);
				}
			}
			else
			{
				for (int i = 0; i < customisableArmour.Count; i++)
				{
					var armourTransform = customisableArmour[i];
					var meshRenderer = armourTransform.GetComponent<SkinnedMeshRenderer>();
					if (meshRenderer == null || meshRenderer.material == null)
						continue;
					SetMaterialHighlighted(false, meshRenderer.material);
				}
			}
		}

		private static void SetMaterialHighlighted(bool highlight, Material material)
		{
			const float intensity = 0.15f;
			if (highlight)
			{
				material.EnableKeyword("_EMISSION");
				//before we can set the color
				material.SetColor("_EmissionColor", Color.white * intensity);
			}
			else
			{
				material.DisableKeyword("_EMISSION");
			}
		}


		private void OpenColourPicker()
		{
			var armourName = _armourHit.gameObject.name.Split("_").Last().ToUpper();
			var message = $"Choose color: {armourName}";
			if (colourPicker.gameObject.activeInHierarchy)
			{
				colourPicker.headerMessage.text = message;
				return;
			}
			ColorPicker.Create(_selectedMaterial.color, message, SetColor, ColorFinished, true);
		}
		private void SetColor(Color currentColor)
		{
			_selectedMaterial.color = currentColor;
		}
		private static void ColorFinished(Color finishedColor)
		{
			// Debug.Log("You chose the color " + ColorUtility.ToHtmlStringRGBA(finishedColor));
		}
	}
}