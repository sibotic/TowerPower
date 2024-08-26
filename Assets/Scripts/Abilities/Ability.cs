using Unity.VisualScripting;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public LayerMask targetLayer;
    public float amount; //eg damage / healing / knockback
    public float range;
    public float duration;
    public float cooldownTime;

    internal float _nextAllowedCast;
    internal bool _allowedToCast = true;

    public abstract void Trigger();

    internal void Cast(){
        _nextAllowedCast = Time.time + cooldownTime;
        _allowedToCast = false;
        Invoke("AllowedToCast", cooldownTime);
    }

    void AllowedToCast(){
        _allowedToCast = true;
    }
}
