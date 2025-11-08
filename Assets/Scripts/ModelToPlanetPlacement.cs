using UnityEngine;
using System.Collections.Generic;

public class ModelToPlanetPlacement : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;
    [SerializeField] LayerMask planetSurfaceLayer;

#if UNITY_EDITOR
    void Start()
    {
        // Move the models to the surface
        foreach (Transform child in transform)
        {
            GameObject childOBJ = child.gameObject;

            Vector3 directionToPlanet = (planetTransform.position - childOBJ.transform.position).normalized;

            RaycastHit hit;

            if (Physics.Raycast(childOBJ.transform.position, directionToPlanet, out hit, Mathf.Infinity, planetSurfaceLayer))
            {
                childOBJ.transform.position = hit.point;
            }
        }
    }

#endif

}