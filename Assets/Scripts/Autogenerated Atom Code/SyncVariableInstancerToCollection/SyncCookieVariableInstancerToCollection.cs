using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type Cookie to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync Cookie Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncCookieVariableInstancerToCollection : SyncVariableInstancerToCollection<Cookie, CookieVariable, CookieVariableInstancer> { }
}
