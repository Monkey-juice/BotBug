using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bugCore : MonoBehaviour
{
    // Start is called before the first frame update

    public static HashSet<GameObject> bugs = new HashSet<GameObject>();
    public float range;
    public bool inrange;
    public int maxHealth;
    int health;
    public int armor;
    public int damage;
    public bool Attack;
    public float AttackSpeed;
    public float EngageRange;
    public bool isDead = false;
    GameObject target = null;
    float targetD;
    int damping = 2;
    float nextShot = 0;
    public NavMeshAgent agent;

    void Awake()
    {
        health = maxHealth;
        bugs.Add(this.gameObject);
    }


    void OnDestroy()
    {
        bugs.Remove(this.gameObject);
    }


    void Update()
    {

        Collider[] colliders;
        colliders = Physics.OverlapSphere(transform.position, EngageRange);
        Vector3 mpos = this.transform.position;
        if (target != null) { targetD = (mpos - target.transform.position).magnitude; }
        bool found = false;
        foreach (Collider thing in colliders)
        {
            //Debug.Log(colliders.Length);
            if (botCore.bots.Contains(thing.gameObject))
            {
                found = true;
                var d = (mpos - thing.transform.position).sqrMagnitude;

                if (target != null) { if (d > targetD) { target = thing.gameObject; } }

                else { target = thing.gameObject; }

            }
        }

        if (!found) { target = null; }

       

        if (targetD < range) { 
        
            inrange = true ;
            agent.SetDestination(target.transform.position);
        }

        else if (target != null)
        {
            agent.SetDestination(target.transform.position);
            inrange = false;
        }

        else { inrange = false ; }








        if (Attack) { shootAtTarget(); }


        

        if (health < 0) { isDead = true; }

    }


    void shootAtTarget()
    {
        var lookPos = target.transform.position - this.transform.position;

        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * damping);

        nextShot += Time.deltaTime;

        if (nextShot > AttackSpeed)
        {
            nextShot = 0;

            target.GetComponent<botCore>().ApplyDamage(damage);

        }
    }



    public void ApplyDamage(int dmg)
    {
        health -= (dmg - armor);
    }
}
