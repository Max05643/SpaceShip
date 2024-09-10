using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersController : MonoBehaviour
{
    [SerializeField]
    Material borderMaterial;

    [SerializeField]
    Transform playerObject, bordersSystemObject;


    [SerializeField]
    [Min(0)]
    float radiusToDisplay = 10f, maxRadiusToDisplay = 50f;

    void FixedUpdate()
    {
        borderMaterial.SetVector("_PlayerPos", playerObject.position);
        borderMaterial.SetFloat("_RadiusToDisplay", radiusToDisplay);
        borderMaterial.SetFloat("_MaxRadiusToDisplay", maxRadiusToDisplay);

        bordersSystemObject.position = new Vector3(0, 0, playerObject.position.z);
    }
}
