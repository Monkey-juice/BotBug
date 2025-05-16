using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class damageable : MonoBehaviour
{

    public UnityEvent<int> OnDamaged;

    public void ApplyDamage(int amount) => OnDamaged?.Invoke(amount);

}
