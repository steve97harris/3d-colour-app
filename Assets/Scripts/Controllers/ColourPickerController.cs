using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace DefaultNamespace
{
	public class ColourPickerController : MonoBehaviour
	{
		private Material _selectedMaterial;

		public void OpenColourPicker(ColorPicker colourPicker, Transform armourHit, Material material)
		{
			var armourName = armourHit.gameObject.name.Split("_").Last().ToUpper();
			var message = $"Choose color: {armourName}";
			if (colourPicker.gameObject.activeInHierarchy)
			{
				colourPicker.headerMessage.text = message;
				return;
			}

			ColorPicker.Create(material.color, message, SetColor, ColorFinished, true);
		}

		public void SetSelectedMaterial(Material material)
		{
			_selectedMaterial = material;
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