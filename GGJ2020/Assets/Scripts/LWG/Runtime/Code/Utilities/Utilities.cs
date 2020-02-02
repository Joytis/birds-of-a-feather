using UnityEngine;

// This class holds useful utility functions that more than one component might want access to (none right now?)
namespace LWG {
public static class Utilities {
	public static int NumberOfSetBits(int i) {
        i = i - ((i >> 1) & 0x55555555);
        i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
        return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
    }
}
} // namespace LWG