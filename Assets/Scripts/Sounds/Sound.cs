namespace TowerDefence
{
    public enum Sound
    {
        BGM_Menu = 0,
        BGM_Level = 1,
        Click = 2,
        Win = 3,
        Lose = 4,
        PlayerLoseHP = 5,
        Arrow = 6,
        ArrowHit = 7,
        EnemyDie = 8,
        ExplosionAbility = 9,
        SlowEnemyAbility = 10,
    }

    public static class SoundExtentions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound, SoundPlayer.Instance.SoundsVolume);
        }
    }
}