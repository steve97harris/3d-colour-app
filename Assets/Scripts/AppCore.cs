using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DefaultNamespace
{
	public class AppCore : MonoBehaviour
	{
		private Color highlightColour = Color.green;
		public List<GameObject> customisableArmour;
		
		private Ray _ray;
		private Material _activeMaterial;
		private Color _prevMaterialColour;

		private void Update()
		{
			if (Camera.main == null)
				return;
			
			_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var found = Physics.Raycast(_ray, out var res);
			if (!found)
			{
				if (_activeMaterial != null && _prevMaterialColour != default)
					_activeMaterial.color = _prevMaterialColour;
				return;
			}

			var meshRenderer = res.transform.GetComponent<SkinnedMeshRenderer>();
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
			Debug.LogError($"{res.transform.name.Split("_").Last()}");
		}
	}
}