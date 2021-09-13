using UnityEngine;

namespace Motorcycle
{
    public static class Extensions
    {
        public static void Reset(this Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0;
        }
    }
}