using Harmony;
using UBOAT.Game.Core.Time;
using UBOAT.Game.Scene.Entities;
using UBOAT.ModAPI.Core.InjectionFramework;
using UnityEngine;

namespace UBOAT.Mods.ScriptingSamples
{
	[HarmonyPatch(typeof(Torpedo), "FixedUpdate")]
	public class TorpedoPatch
	{
		[Inject] private static GameTime gameTime;
		[Inject] private static PlayerShip playerShip;

		/*
		 * This patch rotates all torpedoes towards the player's u-boat after 14 seconds from the launch.
		 *
		 * It's applied as a postfix to FixedUpdate method. FixedUpdate is a special method in Unity that is executed on all objects before each physics time step.
		 *
		 * Second argument on this method is a private field from the original Torpedo class. Harmony allows to access such fields by adding three underscores before the name (example: launchTime becomes then ___launchTime).
		 * Read more here: https://harmony.pardeike.net/articles/patching-injections.html
		 */
		private static void Postfix(Torpedo __instance, ref long ___launchTime)
		{
			long ticksSinceLaunch = gameTime.Ticks - ___launchTime;

			if (ticksSinceLaunch > DateTimeConstants.TicksPerSecond * 14)
			{
				// rotate torpedo
				Vector3 toPlayer = playerShip.transform.position - __instance.transform.position;
				__instance.transform.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
			}
		}
	}
}
