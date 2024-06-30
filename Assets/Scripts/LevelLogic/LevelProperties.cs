using UnityEngine;

namespace SpaceShooter
{
    [System.Serializable]
    public class LevelProperties
    {
        /*[SerializeField] private string m_LevelNum;
        [SerializeField] private string m_Title;
        [SerializeField] private string m_LevelObjective;*/
        [SerializeField] private string m_SceneName;
        /*[SerializeField] private Sprite m_PreviewImage;
        [SerializeField] private LevelProperties m_NextLevel;*/

        [SerializeField] private int m_LevelScore;

        /*
        public string LevelNum => m_LevelNum;
        public string Title => m_Title;
        public string LevelObjective => m_LevelObjective;
        public Sprite PreviewImage => m_PreviewImage;
        */

        public string SceneName { get => m_SceneName; set => m_SceneName = value; }

        public int LevelScore { get => m_LevelScore; set => m_LevelScore = value; }
    }
}