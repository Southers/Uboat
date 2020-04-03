using System;
using System.Reflection;
using DWS.Common.InjectionFramework;
using DWS.Common.Resources;
using Harmony;
using UBOAT.Game.Core.Serialization;
using UBOAT.Game.UI;
using UBOAT.Game.UI.DevConsole;
using UBOAT.ModAPI;
using UBOAT.ModAPI.Core.InjectionFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UBOAT.Mods.ScriptingSamples
{
	/*
	 * This is the entry script for this scripting mod. It sets everything in motion.
	 */
	[NonSerializedInGameState]
	public class ScriptingSamplesMod : IUserMod
	{
		[Inject] private static GameUI gameUI;
		[Inject] private static ResourceManager resourceManager;

		/*
		 * This method is executed right after the mod is loaded by the game.
		 */
		public void OnLoaded()
		{
			// logged text can be read in output_log.txt file in LocalLow folder; it should also appear in the in-game console
			Debug.Log("Hello! Scripting samples mod is loaded.");

			// tell Harmony to apply patches from this mod
			var harmony = HarmonyInstance.Create("com.dws.uboat");
			harmony.PatchAll();

			// register custom commands for the in-game console
			DevConsole.AddListener("HelloWorld", ConsoleCommands.HelloWorld);
			DevConsole.AddListener("Gravity", ConsoleCommands.Gravity);

			// listen for the main scene loading to spawn custom UI
			SceneEventsListener.OnSceneAwake += SceneEventsListener_OnSceneAwake;
		}

		private void SceneEventsListener_OnSceneAwake(Scene scene)
		{
			try
			{
				// it's a workaround for B126 that makes [Inject] tags work more consistently for mods, it won't be necessary in B127
				InjectionFramework.Instance.InjectIntoAssembly(Assembly.GetExecutingAssembly());

				if (scene.name == "Main Scene")
				{
					Debug.Log("Spawning custom UI.");

					// instantiate our custom fps counter prefab here; this prefab is packed into Unity's asset bundle; the source file that is packed resides in "Assets/Packages/scripting-samples/UI" folder.
					var fpsCounter = resourceManager.InstantiatePrefab("UI/FpsCounterUI");

					// parent our custom UI to the main canvas
					fpsCounter.transform.SetParent(gameUI.transform, false);
				}
			}
			catch (Exception e)
			{
				// it's best to catch all exceptions in event listeners; they should preferrably not throw exceptions in C# due to unpredictable results of that
				Debug.LogException(e);
			}
		}
	}
}
