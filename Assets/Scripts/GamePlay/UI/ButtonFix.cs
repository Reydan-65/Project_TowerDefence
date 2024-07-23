using UnityEngine;
namespace TowerDefence
{
    public class ButtonFix : MonoBehaviour
    {
        public void EX_FixAnimation()
        {
            Animator animator = GetComponent<Animator>();
            animator.CrossFade("Normal", 0f);
            animator.Update(0f);
        }
    }
}