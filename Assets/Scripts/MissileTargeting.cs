using UnityEngine;

public class MissileTargeting : MonoBehaviour
{
    public Transform TargetedEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindEnemy();
    }

    void FindEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (var i in Enemies)
        {
            float distance = Vector3.Distance(transform.position, i.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = i;
            }
        }

        if (closest != null)
            TargetedEnemy = closest.transform;
    }
}
