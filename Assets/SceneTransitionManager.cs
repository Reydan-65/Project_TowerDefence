using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private Animator m_Transition;

        private void Start()
        {
            PlayTransitionAnimation("StartScene");
        }

        public void PlayTransitionAnimation(string triggerName)
        {
            if (m_Transition != null)
            {
                m_Transition.SetTrigger(triggerName);
            }
        }
        private float GetAnimationDuration(string animationClipName)
        {
            foreach (AnimationClip clip in m_Transition.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationClipName)
                {
                    return clip.length;
                }
            }

            return 0f;
        }

        public void LoadScene(string sceneName)
        {
            m_Transition.ResetTrigger("EndScene");

            PlayTransitionAnimation("EndScene");

            StartCoroutine(LoadSceneAfterDelay(sceneName));
        }

        private IEnumerator LoadSceneAfterDelay(string sceneName)
        {
            yield return new WaitForSeconds(GetAnimationDuration("sceneTransition_End"));

            SceneManager.LoadScene(sceneName);
        }
    }
}