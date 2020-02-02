using UnityEngine;


namespace LWG {
	public static class VectorExtensions {
		public static void Deconstruct(this Vector2 vec, out float x, out float y) {
			x = vec.x; y = vec.y;
		}

		public static void Deconstruct(this Vector3 vec, out float x, out float y, out float z) {
			x = vec.x; y = vec.y; z = vec.z;
		}

		public static void Deconstruct(this Vector4 vec, out float x, out float y, out float z, out float w) {
			x = vec.x; y = vec.y; z = vec.z; w = vec.w;
		}

		public static void Deconstruct(this Vector2Int vec, out int x, out int y) {
			x = vec.x; y = vec.y;
		}

		public static void Deconstruct(this Vector3Int vec, out int x, out int y, out int z) {
			x = vec.x; y = vec.y; z = vec.z;
		}
	}
}