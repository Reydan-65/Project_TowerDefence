using UnityEngine;
using TowerDefence;

namespace Common
{
    public enum EnemyType
    {
        Primary,
        Secondary,
        Auto
    }

    public abstract class EnemyAssetBase : ScriptableObject { }
}