using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystemBase : MonoBehaviour
{
    public void copyPasteComponents(GameObject prefab)
    {
        EntityCopiableAbstract[] copyScripts = prefab.GetComponents<EntityCopiableAbstract>();
        //Debug.Log(prefab.GetComponents<MonsterEntityAbstractBase>().Lengh) ;
        List<EntityCopiableAbstract> newScripts = new List<EntityCopiableAbstract>();

        foreach (EntityCopiableAbstract copyScript in copyScripts)
        {
            System.Type type = copyScript.GetType();
            Component copy = gameObject.AddComponent(type);

            ((EntityCopiableAbstract)copy).enabled = copyScript.enabled;
            ((EntityCopiableAbstract)copy).pasteScript(copyScript);
            newScripts.Add((EntityCopiableAbstract)copy);
        }
        foreach (EntityCopiableAbstract script in newScripts)
        {
            script.assignScripts();
        }
    }

    public EntityCopiableAbstract copyPasteScript(EntityCopiableAbstract copyScript)
    {
        if(copyScript == null)
        {
            Debug.Log("Aborted: copyScript was null!");
        }

        System.Type type = copyScript.GetType();
        Component copy = gameObject.AddComponent(type);

        ((EntityCopiableAbstract)copy).enabled = copyScript.enabled;
        ((EntityCopiableAbstract)copy).pasteScript(copyScript);
        ((EntityCopiableAbstract)copy).assignScripts();

        return (EntityCopiableAbstract)copy;
    }
}
