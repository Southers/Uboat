using UnityEngine;

namespace UBOAT.Mods.ScriptingSamples
{
	public class ConsoleCommands
	{
		public static void HelloWorld(string[] arguments)
		{
			Debug.Log("Hello world!");
		}

		public static void Gravity(string[] arguments)
		{
			if (arguments.Length < 2)
			{
				Debug.Log("This command sets gravity to the specified value.\nGravity [Value]");
				return;
			}

			float gravity;

			if (float.TryParse(arguments[1], out gravity))
				Physics.gravity = new Vector3(0.0f, -gravity, 0.0f);
			else
				Debug.LogError("Couldn't parse gravity value.");
		}
	}
}
