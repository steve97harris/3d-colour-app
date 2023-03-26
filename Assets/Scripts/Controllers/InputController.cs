using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace DefaultNamespace
{
	public class InputController : MonoBehaviour
	{
		[SerializeField] private InputActionReference _pressAction, _holdAction, _screenPosAction, _axisAction;
		
		private static AppCore AppCore => AppCore.Instance;
		
		[HideInInspector] public Transform ArmourClicked;

		private Transform _lastArmourHit;
		private Vector3 _curScreenPos;
		private bool _isArmourHit
		{
			get
			{
				var ray = AppCore.Instance.mainCam.ScreenPointToRay(_curScreenPos);
				var isArmourHit = Physics.Raycast(ray, out var hit) && AppCore.characterController.customisableArmour.Contains(hit.transform);
				if (isArmourHit)
					_lastArmourHit = hit.transform;
				return isArmourHit;
			}
		}

		public void InitializeInputActions()
		{
			_pressAction.action.Enable();
			_screenPosAction.action.Enable();
			_axisAction.action.Enable();
			_holdAction.action.Enable();

			_screenPosAction.action.performed += TriggerScreenPositionAction;
			_axisAction.action.performed += TriggerAxisAction;
			_pressAction.action.performed += TriggerPressAction;
			_holdAction.action.performed += TriggerHoldAction;
			
			_holdAction.action.canceled += TriggerHoldActionCancelled;
		}

		private void TriggerScreenPositionAction(InputAction.CallbackContext context)
		{
			_curScreenPos = context.ReadValue<Vector2>();
			HighlightExtensions.HighlightCustomisableArmour(_isArmourHit, _lastArmourHit, AppCore.characterController.customisableArmour);
		}

		private void TriggerAxisAction(InputAction.CallbackContext context)
		{
			AppCore.characterController.rotation = context.ReadValue<Vector2>();
		}

		private void TriggerPressAction(InputAction.CallbackContext context)
		{
			if (!_isArmourHit)
				return;

			ArmourClicked = _lastArmourHit;
			var material = ArmourClicked.transform.GetComponent<SkinnedMeshRenderer>().material;
			AppCore.colourPickerController.SetSelectedMaterial(material);
			AppCore.colourPickerController.OpenColourPicker(AppCore.colourPicker, ArmourClicked, material);
		}

		private void TriggerHoldAction(InputAction.CallbackContext context)
		{
			if (_isArmourHit)
				StartCoroutine(AppCore.characterController.RotateCharacter());
		}

		private void TriggerHoldActionCancelled(InputAction.CallbackContext context)
		{
			AppCore.characterController.isDragging = false;
		}

	}
}