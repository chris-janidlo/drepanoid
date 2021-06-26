using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityAtoms.BaseAtoms;

public class KillZone : MonoBehaviour
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

