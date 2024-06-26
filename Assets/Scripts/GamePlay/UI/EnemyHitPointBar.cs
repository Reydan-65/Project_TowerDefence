using SpaceShooter;

namespace TowerDefence
{
    public class EnemyHitPointBar : HitPointBar
    {
        protected override void Update()
        {
            base.Update();
            if (m_Unit != null)
            {
                float hitPoints = ((float)transform.root.GetComponent<Destructible>().HitPoints /
                               (float)transform.root.GetComponent<Destructible>().MaxHitPoints);

                HitPointBarUpdate(hitPoints);
            }
        }
    }
}