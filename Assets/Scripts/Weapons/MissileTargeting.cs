using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileTargeting : MonoBehaviour
{
    public Transform targetedEnemy;

    [Header("Cone Settings")]
    public float ConeAngle = 30f;
    public float Range = 50f;

    [SerializeField] private float MissileCooldown = 4f;
    [SerializeField] public float Cooldown = 0f;

    public bool LockedOn = false;
    GameObject Target;
    public GameObject Missile;
    public Transform MissileSpawn;

    public HUDManager hudManager;

    [Header("Audio")]
    [SerializeField] private AudioClip MissileLaunch;
    [SerializeField] private AudioSource MissileAudioSource;

    void Update()
    { 
        Cooldown -= Time.deltaTime;
        if (targetedEnemy != null)
        {
            if (!LockedOn)
            {
                LockOn(targetedEnemy);
                LockedOn = true;
            }
            else
            {
                checkStillInRange(targetedEnemy);
                if (Input.GetKeyDown(KeyCode.Alpha3) && Cooldown <= 0f)
                {
                    Debug.Log("Missile Fired");
                    ShootMissile();
                    MissileAudioSource.PlayOneShot(MissileLaunch);
                    Cooldown = MissileCooldown;
                    targetedEnemy = null;
                    LockedOn = false;
                }
            }
        }
        else FindEnemy();
    }

    private void FixedUpdate()
    {
        if (!LockedOn)
        {
            FindEnemy();
        }
    }

    void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null || enemies.Length == 0)
        {
            targetedEnemy = null;
            return;
        }

        targetedEnemy = ConeCheck(enemies);
    }
    //FindEnemy finds all enemies in the room and adds them to a list. 
    //ConeCheck is then called if the list is not Null, which acts as a minor optimisation for when no enemies are present.
    //The list containing the transforms of enemies is then passed through to ConeCheck, which checks if they are within range.

    Transform ConeCheck(GameObject[] enemies)
    {
        float halfAngle = ConeAngle * 0.5f;
        List<Transform> enemiesInCone = new List<Transform>();

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (angleToEnemy <= ConeAngle / 2 && distanceToEnemy <= Range)
            {
                enemiesInCone.Add(enemy.transform);
            }
        }

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var enemy in enemiesInCone)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;

        //The cone logic is fairly simple. A new list is created to store enemies within the cone.
        //For each enemy in the original list, the angle and distance to the player is calculated.
        //Using Vector3.Angle, the angle between the player's forward direction and the direction to the enemy is found and compared to the cone angle.
        //If both the angle and distance checks pass, the enemy is added to the new list
        //Finally, the closest enemy from the enemiesInCone list is determined and returned as the TargetedEnemy.
    }

    void checkStillInRange(Transform targetedEnemy)
    {
        float distance = Vector3.Distance(transform.position, targetedEnemy.transform.position);
        Vector3 directionToEnemy = (targetedEnemy.transform.position - transform.position).normalized;
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
        float distanceToEnemy = Vector3.Distance(transform.position, targetedEnemy.transform.position);
        if (angleToEnemy > ConeAngle / 2 || distanceToEnemy > Range)
        {
            LockedOn = false;
            targetedEnemy = null;
        }
    }

    void LockOn(Transform targetedEnemy)
    {
        GameObject target = targetedEnemy.gameObject;
    }

    void ShootMissile()
    {
        GameObject missile = Instantiate(Missile, MissileSpawn.position, MissileSpawn.rotation);
        hudManager.MissileCooldown();
        missile.GetComponent<HomingMissile>().target = targetedEnemy.gameObject.transform;
    }



}
