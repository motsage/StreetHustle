using UnityEngine;

public class IKAnim : MonoBehaviour
{
    public Animator animator;
    public Transform rightHandTarget;
    public Transform leftHandTarget;
    public Transform lookTarget;

    [Range(0, 1)] public float handWeight = 1f;  // How strongly hands follow targets
    [Range(0, 1)] public float lookWeight = 1f;  // How strongly head looks at target

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // Head look
        if (lookTarget != null)
        {
            animator.SetLookAtWeight(lookWeight);
            animator.SetLookAtPosition(lookTarget.position);
        }

        // Right hand
        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }

        // Left hand
        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
}
