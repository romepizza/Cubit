using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMaterials : MonoBehaviour
{

    [Header("----- SETTINGS -----")]
    [Header("--- (Neutral) ---")]
    // 0
    public Material m_MATERIAL_INACTIVE;
    private static Material[] s_MATERIALS_INACTIVE;
    // 1
    public Material m_MATERIAL_ACTIVE_NEUTRAL;
    public static Material[] s_MATERIALS_ACTIVE_NEUTRAL;

    [Header("--- (Player) ---")]
    // 2
    public Material m_MATERIAL_ACTIVE_PLAYER;
    public static Material[] s_MATERIALS_ACTIVE_PLAYER;
    // 3
    public Material m_MATERIAL_ATTACHED_PLAYER;
    public static Material[] s_MATERIALS_ATTACHED_PLAYER;

    [Header("--- (Enemy) ---")]
    [Header("- (Ejector) -")]
    // 4
    public Material m_MATERIAL_ACTIVE_ENEMY_EJECTOR;
    public static Material[] s_MATERIALS_ACTIVE_ENEMY_EJECTOR;
    // 5
    public Material m_MATERIAL_ATTACHED_ENEMY_EJECTOR;
    public static Material[] s_MATERIALS_ATTACHED_ENEMY_EJECTOR;
    // 6
    public Material m_MATERIAL_CORE_ENEMY_EJECTOR;
    public static Material[] s_MATERIALS_CORE_ENEMY_EJECTOR;

    [Header("- (Worm) -")]
    // 7
    public Material m_MATERIAL_ATTACHED_ENEMY_WORM;
    public static Material[] s_MATERIALS_ATTACHED_ENEMY_WORM;
    // 8
    public Material m_MATERIAL_CORE_ENEMY_WORM;
    public static Material[] s_MATERIALS_CORE_ENEMY_WORM;

    [Header("----- DEBUG -----")]
    public int m_minBrightness;

    //private static int cubeNumber = 0;
    private static CubeEntityMaterials s_Instance = null;
    private string m_objectName = "CubeScriptObject";

    // Use this for initialization
    void Start()
    {    
        m_objectName = this.gameObject.name;
        m_minBrightness = (int)(m_MATERIAL_INACTIVE.GetColor("_EmissionColor").maxColorComponent * 100.001f);


        createMaterials();
    }


    void createMaterials()
    {
        createInactiveMaterial();
        createActiveNeutralMaterial();
        createActivePlayerMaterial();
        createAttachedPlayerMaterial();
        createActiveEnemyEjectorMaterial();
        createAttachedEnemyEjectorMaterial();
        createCoreEjectorMaterial();
        createAttachedEnemyWormMaterial();
        createCoreWormMaterial();
    }
    public static Material getMaterial(CubeEntityAppearance appearanceScript, float factor)
    {
        if(appearanceScript.m_material == getInstance().m_MATERIAL_ACTIVE_NEUTRAL)
        {
           return CubeEntityMaterials.getActiveNeutralMaterial(factor);
        }
        if (appearanceScript.m_material == getInstance().m_MATERIAL_ACTIVE_PLAYER)
        {
            return CubeEntityMaterials.getActivePlayerMaterial(factor);
        }
        if (appearanceScript.m_material == getInstance().m_MATERIAL_ACTIVE_ENEMY_EJECTOR)
        {
            return CubeEntityMaterials.getActiveEnemyEjectorMaterial(factor);
        }

        return null;
    }

    // Neutral
    void createInactiveMaterial()
    {
        if(m_MATERIAL_INACTIVE != null)
        {
            
        }
    }
    void createActiveNeutralMaterial()
    {
        if (m_MATERIAL_ACTIVE_NEUTRAL != null)
        {
            int brightness = (int)(m_MATERIAL_ACTIVE_NEUTRAL.GetColor("_EmissionColor").maxColorComponent * 100f);
            if (brightness <= 0)
            {
                Debug.Log("Warning: brightness less equal to zero! (" + brightness + ")");
                return;
            }
            int adjustedBrightness = brightness - m_minBrightness;
            adjustedBrightness = Mathf.Max(adjustedBrightness, 0);
            s_MATERIALS_ACTIVE_NEUTRAL = new Material[adjustedBrightness + 1];
            s_MATERIALS_ACTIVE_NEUTRAL[adjustedBrightness] = m_MATERIAL_ACTIVE_NEUTRAL;
            for (int i = adjustedBrightness - 1; i >= 0; i--)
            {
                Material tmpMat = new Material(m_MATERIAL_ACTIVE_NEUTRAL);
                float multiplier = (i + m_minBrightness) / (float)(brightness);
                tmpMat.SetColor("_EmissionColor", tmpMat.GetColor("_EmissionColor") * multiplier);
                s_MATERIALS_ACTIVE_NEUTRAL[i] = tmpMat;
            }
        }
    }
    public static Material getActiveNeutralMaterial(float factor)
    {
        factor = Mathf.Clamp01(factor);
        int index = (int)(factor * (s_MATERIALS_ACTIVE_NEUTRAL.Length - 1));
        return s_MATERIALS_ACTIVE_NEUTRAL[index];
    }

    // Player
    void createActivePlayerMaterial()
    {
        if (m_MATERIAL_ACTIVE_PLAYER != null)
        {
            int brightness = (int)(m_MATERIAL_ACTIVE_PLAYER.GetColor("_EmissionColor").maxColorComponent * 100f);
            if(brightness <= 0)
            {
                Debug.Log("Warning: brightness less equal to zero! (" + brightness + ")");
                return;
            }
            int adjustedBrightness = brightness - m_minBrightness;
            adjustedBrightness = Mathf.Max(adjustedBrightness, 0);
            s_MATERIALS_ACTIVE_PLAYER = new Material[adjustedBrightness + 1];
            s_MATERIALS_ACTIVE_PLAYER[adjustedBrightness] = m_MATERIAL_ACTIVE_PLAYER;
            for(int i = adjustedBrightness - 1; i >= 0; i--)
            {
                Material tmpMat = new Material(m_MATERIAL_ACTIVE_PLAYER);
                float multiplier = (i + m_minBrightness) / (float)(brightness);
                tmpMat.SetColor("_EmissionColor", tmpMat.GetColor("_EmissionColor") * multiplier);
                s_MATERIALS_ACTIVE_PLAYER[i] = tmpMat;
            }
        }
    }
    public static Material getActivePlayerMaterial(float factor)
    {
        factor = Mathf.Clamp01(factor);
        int index = (int)(factor * (s_MATERIALS_ACTIVE_PLAYER.Length - 1));

        return s_MATERIALS_ACTIVE_PLAYER[index];
    }
    void createAttachedPlayerMaterial()
    {

    }

    // Ejector
    void createActiveEnemyEjectorMaterial()
    {
        if (m_MATERIAL_ACTIVE_ENEMY_EJECTOR != null)
        {
            int brightness = (int)(m_MATERIAL_ACTIVE_ENEMY_EJECTOR.GetColor("_EmissionColor").maxColorComponent * 100f);
            if (brightness <= 0)
            {
                Debug.Log("Warning: brightness less equal to zero! (" + brightness + ")");
                return;
            }
            int adjustedBrightness = brightness - m_minBrightness;
            adjustedBrightness = Mathf.Max(adjustedBrightness, 0);
            s_MATERIALS_ACTIVE_ENEMY_EJECTOR = new Material[adjustedBrightness + 1];
            s_MATERIALS_ACTIVE_ENEMY_EJECTOR[adjustedBrightness] = m_MATERIAL_ACTIVE_ENEMY_EJECTOR;
            for (int i = adjustedBrightness - 1; i >= 0; i--)
            {
                Material tmpMat = new Material(m_MATERIAL_ACTIVE_ENEMY_EJECTOR);
                float multiplier = (i + m_minBrightness) / (float)(brightness);
                tmpMat.SetColor("_EmissionColor", tmpMat.GetColor("_EmissionColor") * multiplier);
                s_MATERIALS_ACTIVE_ENEMY_EJECTOR[i] = tmpMat;
            }
        }
    }
    public static Material getActiveEnemyEjectorMaterial(float factor)
    {
        factor = Mathf.Clamp01(factor);
        int index = (int)(factor * (s_MATERIALS_ACTIVE_ENEMY_EJECTOR.Length - 1));
        return s_MATERIALS_ACTIVE_ENEMY_EJECTOR[index];
    }
    void createAttachedEnemyEjectorMaterial()
    {

    }
    void createCoreEjectorMaterial()
    {

    }

    // Worm
    void createAttachedEnemyWormMaterial()
    {

    }
    void createCoreWormMaterial()
    {

    }

    

    public static CubeEntityMaterials getInstance()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(CubeEntityMaterials)) as CubeEntityMaterials;
        }

        if (s_Instance == null)
        {
            Debug.Log("Singleton of CubeEntityMaterials not working properly!");
        }
        return s_Instance;
    }
}
