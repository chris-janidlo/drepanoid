using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Cookie Data", fileName = "newCookie.asset")]
public class Cookie : ScriptableObject, IEquatable<Cookie>
{
    public string Header;
    [TextArea(5, 500)]
    public string Data;

    public bool Equals (Cookie other)
    {
        return base.Equals(other);
    }
}
