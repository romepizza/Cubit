using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityPlasmaProjectile : MonsterEntityAbstractBase
{

    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    [Header("--- (Scripts) ---")]
    public bool placeHolder;



    private void OnCollisionEnter(Collision collision)
    {
        GameObject colliderGameObject = collision.gameObject;
        if (colliderGameObject.GetComponent<CubeEntitySystem>() != null)
        {
            if(colliderGameObject.GetComponent<CubeEntityState>().m_affiliation != GetComponent<CubeEntityState>().m_affiliation)
            {
                GetComponent<MonsterEntityBase>().die();
            }
        }
    }

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {

    }
    public override void prepareDestroyScript()
    {
        Constants.getMainCge().GetComponent<CgeMonsterManager>().deregisterEnemy(this);
        Destroy(this);
    }
    public override void assignScripts()
    {

    }
}
