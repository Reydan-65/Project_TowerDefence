using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class StandUp : MonoBehaviour
    {
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private void Start()
        {
            rb = transform.root.GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }
        private void LateUpdate()
        {
            transform.up = Vector2.up;

            var xMotion = rb.velocity.x;

            if (xMotion > 0.01f) sr.flipX = false;
            else if (xMotion < 0.01f) sr.flipX = true;
        }
    }
}