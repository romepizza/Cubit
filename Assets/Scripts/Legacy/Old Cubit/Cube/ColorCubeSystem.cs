using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCubeSystem : MonoBehaviour
{
    [Header("----- Settings -----")]
    [Header("--- (Initialize) ---")]
    public bool initializeCubesGo;

    [Header("--- (Color) ---")]
    public bool colorToDefault;
    public bool colorToRandom;
    public Material defaultMaterial;
    public Material[] randomMaterialsNormal;
    public Material[] randomMaterialsEmissive_25;
    public Material[] randomMaterialsEmissive_50;

    [Header("--- (FX) ---")]
    public GameObject effectObject;

    [Header("--- (Light) ---")]
    public GameObject lightObject;
    public bool addLights;
    public bool removeLights;
    public float lightDuration;
    public float lightDurationRandomBonus;
    public float chanceOnLightCube;

    [Header("--- (Trail Renderer) ---")]
    public Material trailMaterial;
    public bool addTrailRenderer;
    public bool removeTrailRenderer;
    public bool useDefaultMaterial;

    [Header("--- (Halo)---")]
    public GameObject haloPrefab;
    public bool addHalo;
    public bool removeHalo;
    public float haloSize;

    void Start ()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<Rigidbody>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchAtStart)
            {
                cube.gameObject.GetComponent<Rigidbody>().velocity = new Vector3((Random.Range(0, 200) - 100) * 0.01f, (Random.Range(0, 200) - 100) * 0.01f, (Random.Range(0, 200) - 100) * 0.01f);
                cube.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3((Random.Range(0, 200) - 100) * 0.1f, (Random.Range(0, 200) - 100) * 0.1f, (Random.Range(0, 200) - 100) * 0.1f), ForceMode.Acceleration);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void initializeCubes()
    {
       // initializeCubes
    }

    void initializeCube(GameObject cube)
    {
        if (cube.GetComponent<Rigidbody>() == null)
            cube.AddComponent<Rigidbody>();
        //if(cube.GetComponent<TrailRenderer>() == null)
    }

    void setColorToDefault()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<MeshRenderer>() != null && cube.gameObject.GetComponent<ColorCube>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchMaterial)
                cube.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    void setColorToRandom()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<MeshRenderer>() != null && cube.gameObject.GetComponent<ColorCube>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchMaterial)
            {
                int random = (int)Random.Range(0, randomMaterialsNormal.Length);
                cube.gameObject.GetComponent<MeshRenderer>().material = randomMaterialsNormal[random];
                cube.gameObject.GetComponent<ColorCube>().materialInactive = randomMaterialsNormal[random];
                cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().lightDurationActual = lightDuration + Random.Range(0, lightDurationRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer = randomMaterialsEmissive_50[random];
                cube.gameObject.GetComponent<ColorCube>().materialGrabbedByPlayer = randomMaterialsEmissive_25[random];
            }
        }
    }

    void addLightToCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {

                if (cube.gameObject.GetComponent<ColorCube>() != null)
                    cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);
                bool effectFound = false;
                bool lightFound = false;
                foreach (Transform child in cube)
                {
                    if (child.gameObject.GetComponent<Light>() != null)
                        lightFound = true;
                    if (child.gameObject.GetComponent<ParticleSystem>() != null)
                        effectFound = true;
                }

                if (Random.Range((float)0, (float)1) < chanceOnLightCube)
                {
                    if (!lightFound && lightObject != null)
                        Instantiate(lightObject, cube);
                    if (!effectFound && effectObject != null)
                        Instantiate(effectObject, cube);
                }

                foreach (Transform child in cube)
                {
                    Color color = cube.GetComponent<Renderer>().sharedMaterial.color;
                    if (child.gameObject.GetComponent<Light>() != null)
                    {
                        child.gameObject.GetComponent<Light>().color = color;
                        child.gameObject.SetActive(false);
                    }
                    if (child.gameObject.GetComponent<ParticleSystem>() != null)
                        child.localPosition = Vector3.zero;

                }
            }
        }
    }

    void removeLightFromCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                foreach (Transform child in cube)
                {
                    if (child != null && child.gameObject != null && child.gameObject.GetComponent<Light>() != null)
                        DestroyImmediate(child.gameObject);

                    if (child != null && child.gameObject != null && child.gameObject.GetComponent<ParticleSystem>() != null)
                        DestroyImmediate(child.gameObject);
                }
            }
        }
    }

    void addTrailRendererToCube()
    {
        foreach(Transform cube in transform)
        {
            if(cube.gameObject.GetComponent<ColorCube>() != null)
            {
                if (cube.gameObject.GetComponent<TrailRenderer>() == null)
                    cube.gameObject.AddComponent<TrailRenderer>();
                if(useDefaultMaterial)
                    cube.gameObject.GetComponent<TrailRenderer>().material = cube.GetComponent<ColorCube>().materialActiveByPlayer;
                else
                    cube.gameObject.GetComponent<TrailRenderer>().material = trailMaterial;
                cube.gameObject.GetComponent<TrailRenderer>().time = 0;//cube.gameObject.GetComponent<ColorCube>().lightDurotation;
                cube.gameObject.GetComponent<TrailRenderer>().startColor = cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer.color;
                cube.gameObject.GetComponent<TrailRenderer>().endColor = cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer.color;
            }
        }
    }

    void removeTrailRendererToCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                DestroyImmediate(cube.gameObject.GetComponent<TrailRenderer>());
            }
        }
    }

    void OnDrawGizmos()
    {
        if (colorToDefault)
        {
            setColorToDefault();
            colorToDefault = false;
        }

        if(colorToRandom)
        {
            setColorToRandom();
            colorToRandom = false;
        }

        if(addLights)
        {

            addLightToCube();
            addLights = false;
        }

        if(removeLights)
        {
            removeLightFromCube();
            removeLights = false;
        }


        if (addTrailRenderer)
        {
            addTrailRendererToCube();
            addTrailRenderer = false;
        }

        if (removeTrailRenderer)
        {
            removeTrailRendererToCube();
            removeTrailRenderer = false;
        }
        /*
        if(addHalo)
        {
            foreach (Transform cubes in transform)
            {
                if (cubes.gameObject.name == "LightCubes")
                {
                    foreach (Transform cube in cubes)
                    {
                        //if ((Behaviour)cube.gameObject.GetComponent("Halo") == null)
                        if (cube.gameObject.GetComponent("Halo") == null && haloPrefab != null)
                        {
                            GameObject halo = Instantiate(haloPrefab) as GameObject;
                            halo.transform.SetParent(cube.transform, false);
                        }
                    }
                }
            }


            addHalo = false;
        }

        if(removeHalo)
        {
            foreach (Transform cubes in transform)
            {
                if (cubes.gameObject.name == "LightCubes")
                {
                    foreach (Transform cube in cubes)
                    {
                        if (cube.gameObject.GetComponent("Halo") == null)
                        {
                            foreach (Transform child in cube)
                            {
                                if (child != null && child.gameObject != null && child.gameObject.GetComponent("Halo") != null)
                                    DestroyImmediate(child.gameObject);
                            }
                        }
                    }
                }
            }
            removeHalo = false;
        }
        */
    }
}
