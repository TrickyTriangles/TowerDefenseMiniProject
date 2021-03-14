using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem smoke_particle;
    private DamageProfile damage_profile;

    public void SetInitialValues(DamageProfile new_profile, float radius)
    {
        damage_profile = new_profile;
        transform.localScale *= radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        HitOther(other);
    }

    private void HitOther(Collider other)
    {
        if (other.gameObject.CompareTag("Targetable"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage_profile);
            }
        }
    }

    public void HandleEndOfAnimation()
    {
        Destroy(gameObject);
    }
}
