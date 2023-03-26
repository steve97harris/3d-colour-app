using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
	public static class HighlightExtensions
	{
		public static void HighlightCustomisableArmour(bool isArmourHit, Transform armourHit, List<Transform> customisableArmour)
		{
			if (isArmourHit)
			{
				for (int i = 0; i < customisableArmour.Count; i++)
				{
					var armourTransform = customisableArmour[i];
					var meshRenderer = armourTransform.GetComponent<SkinnedMeshRenderer>();
					if (meshRenderer == null || meshRenderer.material == null)
						continue;
					SetMaterialHighlighted(armourTransform == armourHit, meshRenderer.material);
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
	}
}