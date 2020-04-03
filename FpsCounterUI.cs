using TMPro;
using UBOAT.Game.Core.Serialization;
using UnityEngine;

namespace UBOAT.Mods.ScriptingSamples
{
	[NonSerializedInGameState]
	public class FpsCounterUI : MonoBehaviour
	{
		private void Update()
		{
			GetComponent<TextMeshProUGUI>().text = (1.0f / Time.unscaledDeltaTime).ToString("0");
		}
	}
}
