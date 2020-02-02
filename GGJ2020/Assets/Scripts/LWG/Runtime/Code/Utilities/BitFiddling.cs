namespace LWG {
	public static class BitFiddling {
		public static bool IsPowerOfTwo(ulong x) => (x != 0) && ((x & (x - 1)) == 0);
	}
}