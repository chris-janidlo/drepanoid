using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

[CreateAssetMenu(menuName = "Text Queue", fileName = "newTextQueue.asset")]
public class TextQueue : ScriptableObject
{
    public delegate IEnumerator Setter (TextEffectData data);
    public delegate void Deleter (Vector2Int regionStartPosition, Vector2Int regionExtents);

    Setter setter;
    Deleter deleter;

    public void RegisterProcessors (Setter setter, Deleter deleter)
    {
        if (this.setter != null || this.deleter != null)
        {
            throw new InvalidOperationException("processors are already registered");
        }

        this.setter = setter;
        this.deleter = deleter;
    }

    public IEnumerator SetText (TextEffectData effectData)
    {
        yield return setter(effectData);
    }

    public void Delete (Vector2Int regionStartPosition, Vector2Int regionExtents)
    {
        deleter(regionStartPosition, regionExtents);
    }
}
