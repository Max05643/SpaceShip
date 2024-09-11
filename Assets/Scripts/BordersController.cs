using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Controller for displaying borders and visual effects associated with them
/// </summary>
public class BordersController : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        [Min(0)]
        public float radiusToDisplay = 10f, maxRadiusToDisplay = 50f;
    }

    [SerializeField]
    Material borderMaterial;

    [SerializeField]
    Transform playerObject, bordersSystemObject;

    [Inject]
    Settings settings;

    void FixedUpdate()
    {
        borderMaterial.SetVector("_PlayerPos", playerObject.position);
        borderMaterial.SetFloat("_RadiusToDisplay", settings.radiusToDisplay);
        borderMaterial.SetFloat("_MaxRadiusToDisplay", settings.maxRadiusToDisplay);

        bordersSystemObject.position = new Vector3(0, 0, playerObject.position.z);
    }
}
