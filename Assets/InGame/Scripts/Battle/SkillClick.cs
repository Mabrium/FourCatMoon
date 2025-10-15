using UnityEngine;

public class SkillClick : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SkillUI(bool onOff)
    {
        animator.SetBool("SkillMenu", onOff);
    }
}