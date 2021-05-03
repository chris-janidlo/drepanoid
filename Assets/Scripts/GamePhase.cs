using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    LevelLoading, // while loading animation is playing
    LevelPlaying, // while there's a ball on screen the player can control
    LevelCompleted // while the level won animation is playing
}
