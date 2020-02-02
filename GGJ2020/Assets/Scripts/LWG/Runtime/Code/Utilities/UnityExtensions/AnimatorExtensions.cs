using UnityEngine;

namespace LWG.Core.Extensions {
	public static class AnimatorExtensions {
		public static float CurrentClipAdjustedLength(this Animator anim) {
			AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0);
			AnimationClip animClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;
            return animClip.length * animationState.speedMultiplier;
		}

		public static void PlayAndUpdate(this Animator anim, string name, int layer = -1, float offset = 0f) {
			anim.Play(name, layer, offset);
			anim.Update(0.0f); // Force Update
		}

		public static void PlayAtSpeedScale(this Animator anim, 
											string name, 
											float speed, 
											int layer = -1, 
											float offset = 0f) {
	        anim.speed = speed;
			anim.PlayAndUpdate(name, layer, offset);
		}
	}
}