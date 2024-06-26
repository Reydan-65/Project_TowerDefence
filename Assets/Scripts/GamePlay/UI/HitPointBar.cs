using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public abstract class HitPointBar : MonoBehaviour
    {
        [SerializeField] protected Transform m_Unit;
        [SerializeField] protected Image[] m_Image;
        [SerializeField] protected GameObject m_Canvas;

        protected Camera m_Camera;
        protected RectTransform m_CanvasRect;

        protected float lastHitPoints;

        protected void Start()
        {
            m_Camera = FindAnyObjectByType<Camera>();
            m_Canvas.GetComponent<Canvas>().worldCamera = m_Camera;

            m_CanvasRect = m_Canvas.GetComponent<RectTransform>();

            float sizeX = transform.root.GetComponentInChildren<CircleCollider2D>().radius * 2;

            m_CanvasRect.localScale = new Vector2(sizeX, m_CanvasRect.localScale.y);

            for (int i = 0; i < m_Image.Length; i++)
            {
                m_Image[i].enabled = false;
            }
        }

        protected virtual void Update()
        {
            if (m_Unit != null)
            {
            m_Canvas.transform.position = m_Unit.position;

            float angle = m_Unit.rotation.eulerAngles.y;
            m_Canvas.transform.eulerAngles = new Vector3(0, 0, -angle);

                if (m_Unit.transform.root.GetComponent<Destructible>().HitPoints <
                    m_Unit.transform.root.GetComponent<Destructible>().MaxHitPoints)
                {
                    for (int i = 0; i < m_Image.Length; i++)
                    {
                        m_Image[i].enabled = true;
                    }
                }
            }
        }

        protected void HitPointBarUpdate(float hitPoints)
        {
            if (m_Image[2] != null)
            {
                if (hitPoints != lastHitPoints)
                {
                    m_Image[2].fillAmount = hitPoints;
                    lastHitPoints = hitPoints;
                }

                //Color nearToDeath = Color.Lerp(Color.red, Color.green, m_Image.fillAmount);

                //m_Image.color = nearToDeath;
            }
        }
    }
}