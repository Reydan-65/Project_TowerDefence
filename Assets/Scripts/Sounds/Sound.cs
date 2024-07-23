namespace TowerDefence
{
    public enum Music
    {
        Menu = 0,
        LevelMap = 1,
        Level_01 = 2,
        Level_02 = 3,
        Level_03 = 4,
        Level_04 = 5,
        Level_01_2 = 6,
        Level_02_2 = 7,
        EndGame = 8,
    }

    public enum Sound
    {
        Click = 0,
        Win = 1,
        Lose = 2,
        PlayerLoseHP = 3,
        Arrow = 4,
        ArrowHit = 5,
        FrostArrow = 6,
        FrostArrowHit = 7,
        SiegeArrow = 8,
        SiegeArrowHit = 9,
        EnemyDie = 10,
        ExplosionAbility_Use = 11,
        ExplosionAbility_Active = 12,
        SlowEnemyAbility_Active = 13,
        SlowEnemyAbility_End = 14,
    }

    public static class SoundExtentions
    {
        public static void PlayMusic(this Music music)
        {
            SoundPlayer.Instance.PlayMusic(music, SoundPlayer.Instance.SoundsVolume);
        }

        public static void PlaySound(this Sound sound)
        {
            SoundPlayer.Instance.PlaySound(sound, SoundPlayer.Instance.SoundsVolume);
        }
    }
}