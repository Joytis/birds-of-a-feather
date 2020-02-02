using UnityEngine;

namespace LWG {
	public static class ColorExtensions {
		public static void Deconstruct(this Color color, out float r, out float g, out float b) {
			r = color.r; g = color.g; b = color.b;
		}
	}
}