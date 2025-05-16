using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botCore : MonoBehaviour
{

    public float range;
    public bool inrange;
    public static HashSet<GameObject> bots = new HashSet<GameObject>();
    public int maxHealth;
    public ParticleSystem muzzleFlash;
    public int health;
    public int armor;
    public int damage;
    public bool Attack;
    public float AttackSpeed;
    public bool isDead = false;
    GameObject target = null;
    float targetD;
    int damping = 2;
    float nextShot =0;
    

    //public bugCore target;

    // Update is called once per frame
    void Awake()
    {
        health = maxHealth;
        bots.Add(this.gameObject);
    }



    void OnDisable()
    {
        bots.Remove(this.gameObject);
    }

    void Update()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(transform.position, range);
        Vector3 mpos = this.transform.position;
        if (target != null) { targetD = (mpos - target.transform.position).sqrMagnitude; }
        bool found = false;
        foreach (Collider thing in colliders)
        {
            //Debug.Log(colliders.Length);
            if (bugCore.bugs.Contains(thing.gameObject))
            {
                inrange = true;
                found = true;
                var d = (mpos - thing.transform.position).sqrMagnitude;

                if (target != null) { if (d > targetD) { target = thing.gameObject; } }

                else { target = thing.gameObject; }

            }
        }
        if (!found) { inrange = false; target = null; }


        if (Attack) { shootAtTarget(); }

        if (health < 0) { isDead = true; }

    }


    void shootAtTarget()
    {
        var lookPos = target.transform.position - this.transform.position;

        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime*damping);

        nextShot += Time.deltaTime;

        if (nextShot > AttackSpeed) { 
            nextShot = 0;

            target.GetComponent<bugCore>().ApplyDamage(damage);

            muzzleFlash.Play();
        }
    }

    public void ApplyDamage(int dmg)
    {
        health -= (dmg - armor);
    }



}
