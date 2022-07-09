using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace Drepanoid
{
    public partial class KillZone : MonoBehaviour
    {
        void OnTriggerEnter2D (Collider2D collision)
        {
            Ball ball = collision.GetComponent<Ball>();

            if (ball != null)
            {
                ball.Kill();
            }
        }
    }
}
