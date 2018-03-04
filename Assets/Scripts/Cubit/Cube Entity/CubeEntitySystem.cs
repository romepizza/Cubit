using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntitySystem : MonoBehaviour
{
    public bool doStuff;
    public GameObject test;
    [Header("----- SETTINGS -----")]

    [Header("----- DEBUG -----")]
    public CubeEntityAppearance m_appearanceComponent;
    public CubeEntityCharge m_chargeComponent;
    public CubeEntityCollision m_collisionComponent;
    public CubeEntityMovement m_movementComponent;
    public CubeEntityState m_stateComponent;
    public CubeEntityTransform m_transformComponent;
    public CubeEntityPrefapSystem m_prefapSystemComponent;


    void Start ()
    {
        addComponentsAtStart();
        //getPrefapSystem().setToInactive();
    }

    /*
    void Update ()
    {
        if (doStuff)
        {
            doTestStuff();
            doStuff = false;
        }
    }
        */


    public void addComponentsAtStart()
    {
        if(GetComponent<CubeEntityAppearance>() == null)
            gameObject.AddComponent<CubeEntityAppearance>();
        m_appearanceComponent = GetComponent<CubeEntityAppearance>();
        GetComponent<CubeEntityAppearance>().m_entitySystemComponent = this;

        //if(GetComponent<CubeEntityCharge>() == null)
            //gameObject.AddComponent<CubeEntityCharge>();
        //m_cubeEntityChargeComponent = GetComponent<CubeEntityCharge>();
        //GetComponent<CubeEntityCharge>() = this;

        if(GetComponent<CubeEntityCollision>() == null)
            gameObject.AddComponent<CubeEntityCollision>();
        m_collisionComponent = GetComponent<CubeEntityCollision>();
        GetComponent<CubeEntityCollision>().m_entitySystemScript = this;

        if(GetComponent<CubeEntityMovement>() == null)
           gameObject.AddComponent<CubeEntityMovement>();
        m_movementComponent = GetComponent<CubeEntityMovement>();
        GetComponent<CubeEntityMovement>().m_entitySystemScript = this;

        if(GetComponent<CubeEntityState>() == null)
            gameObject.AddComponent<CubeEntityState>();
        m_stateComponent = GetComponent<CubeEntityState>();
        GetComponent<CubeEntityState>().m_entitySystemScript = this;

        if(GetComponent<CubeEntityTransform>() == null)
            gameObject.AddComponent<CubeEntityTransform>();
        m_transformComponent = gameObject.GetComponent<CubeEntityTransform>();
        GetComponent<CubeEntityTransform>().m_entitySystemScript = this;

        if(GetComponent<CubeEntityPrefapSystem>() == null)
            gameObject.AddComponent<CubeEntityPrefapSystem>();
        m_prefapSystemComponent = GetComponent<CubeEntityPrefapSystem>();
        GetComponent<CubeEntityPrefapSystem>().m_entitySystemScript = this;
    }

    // Prefabs
    public void setToInactive()
    {
        getPrefapSystem().setToInactive();
    }
    public void setToActiveNeutral()
    {
        getPrefapSystem().setToActiveNeutral();
    }
    public void setToActivePlayer(/*Vector3 targetPoint, float duration, float power, float maxSpeed*/)
    {
        getPrefapSystem().setToActivePlayer();
        //getMovementComponent().removeAllMovementComponents();
        //getMovementComponent().addAccelerationComponent(targetPoint, duration, power, maxSpeed);
    }
    public void setToAttachedPlayer(Vector3 targetPoint, float duration, float power, float maxSpeed)
    {
        getMovementComponent().removeAllMovementComponents();
        getPrefapSystem().setToAttachedPlayer();
        //getMovementComponent().addFollowPointComponent(targetPoint, duration, power, maxSpeed);
    }
    public void setToAttachedPlayer()
    {
        getMovementComponent().removeAllMovementComponents();
        getPrefapSystem().setToAttachedPlayer();
    }
    public void setToActiveEnemyEjector(/*Vector3 targetPoint, float duration, float power, float maxSpeed*/)
    {
        getPrefapSystem().setToActiveEnemyEjector();
        //getMovementComponent().removeAllMovementComponents();
        //getMovementComponent().addAccelerationComponent(targetPoint, duration, power, maxSpeed);
    }
    public void setToAttachedEnemyEjector(Vector3 targetPoint, float duration, float power, float maxSpeed)
    {
        getPrefapSystem().setToAttachedEnemyEjector();
        getMovementComponent().removeAllAccelerationComponents();
        getMovementComponent().addFollowPointComponent(targetPoint, duration, power, maxSpeed);
    }
    public void setToAttachedEnemyWorm()
    {
        getPrefapSystem().setToAttachedEnemyWorm();
    }
    public void setToCoreEjector()
    {
        getPrefapSystem().setToCoreEjector();
        getMovementComponent().removeAllAccelerationComponents();

    }
    public void setToCoreWorm()
    {
        getPrefapSystem().setToCoreWorm();
    }


    // Setter
    public bool setAppearanceComponent(GameObject appearanceSettings)
    {
        if (m_appearanceComponent == null)
        {
            if (appearanceSettings.GetComponent<CubeEntityAppearance>() != null)
            {
                gameObject.AddComponent<CubeEntityAppearance>();
                gameObject.GetComponent<CubeEntityAppearance>().m_entitySystemComponent = this;
            }
            m_appearanceComponent = gameObject.GetComponent<CubeEntityAppearance>();
            gameObject.GetComponent<CubeEntityAppearance>().setAppearanceByScript(appearanceSettings);

            return true;
        }
        else
        {
            if (GetComponent<CubeEntityAppearance>() == null)
            {
                Destroy(getAppearanceComponent());
            }
            else
            {
                getAppearanceComponent().setAppearanceByScript(appearanceSettings);
            }
            return false;
        }
    }
    public bool setChargeComponent(GameObject chargeSettings)
    {
        if (m_chargeComponent == null)
        {
            if (chargeSettings.GetComponent<CubeEntityCharge>() == null)
            { 
                gameObject.AddComponent<CubeEntityCharge>();
                m_chargeComponent = gameObject.GetComponent<CubeEntityCharge>();
            }
            // Copy charge settings here

            return true;
        }
        else
        {
            if (chargeSettings.GetComponent<CubeEntityCharge>() == null)
            {

            }
            else
            {
                Destroy(getChargeComponent());
            }
            return false;
        }
    }
    public bool setCollisionComponent(GameObject collisionSettings)
    {
        if (m_collisionComponent == null)
        {
            if (collisionSettings.GetComponent<CubeEntityCollision>() == null)
            {
                gameObject.AddComponent<CubeEntityCollision>();
                gameObject.GetComponent<CubeEntityCollision>().m_entitySystemScript = this;
            }
            m_collisionComponent = gameObject.GetComponent<CubeEntityCollision>();
            // Copy collision settings here

            return true;
        }
        else
        {
            if (collisionSettings.GetComponent<CubeEntityCollision>() == null)
            {

            }
            else
            {
                Destroy(getCollisionComponent());
            }
            return false;
        }
    }
    public bool setMovementComponent(GameObject movementSettings)
    {
        if (m_movementComponent == null)
        {
            if (movementSettings.GetComponent<CubeEntityMovement>() == null)
            {
                gameObject.AddComponent<CubeEntityMovement>();
                m_movementComponent = gameObject.GetComponent<CubeEntityMovement>();
            }
            // Copy movement settings here

            return true;
        }
        else
        {
            if (movementSettings.GetComponent<CubeEntityMovement>() == null)
            {

            }
            else
            {
                Destroy(getMovementComponent());
            }
            return false;
        }
    }
    public bool setStateComponent(GameObject stateSettings)
    {
        if (m_stateComponent == null)
        {
            if (stateSettings.GetComponent<CubeEntityState>() == null)
            {
                gameObject.AddComponent<CubeEntityState>();
                m_stateComponent = gameObject.GetComponent<CubeEntityState>();
            }
            // Copy state settings here

            return true;
        }
        else
        {
            if (stateSettings.GetComponent<CubeEntityState>() == null)
            {

            }
            else
            {
                Destroy(getStateComponent());
            }
            return false;
        }
    }
    public bool setTransformComponent(GameObject transformSettings)
    {
        if (m_transformComponent == null)
        {
            if (transformSettings.GetComponent<CubeEntityTransform>() == null)
            {
                gameObject.AddComponent<CubeEntityTransform>();
                m_transformComponent = gameObject.GetComponent<CubeEntityTransform>();
            }
            // Copy transform settings here

            return true;
        }
        else
        {
            if (transformSettings.GetComponent<CubeEntityTransform>() == null)
            {

            }
            else
            {
                Destroy(getTransformComponent());
            }
            return false;
        }
    }
    public bool setPrefapSystemComponent()
    {
        return false;
    }

    // Getter
    public CubeEntityAppearance getAppearanceComponent()
    {
        return m_appearanceComponent;
    }
    public CubeEntityCharge getChargeComponent()
    {
        return m_chargeComponent;
    }
    public CubeEntityCollision getCollisionComponent()
    {
        return m_collisionComponent;
    }
    public CubeEntityMovement getMovementComponent()
    {
        return m_movementComponent;
    }
    public CubeEntityState getStateComponent()
    {
        return m_stateComponent;
    }
    public CubeEntityTransform getTransformComponent()
    {
        return m_transformComponent;
    }
    public CubeEntityPrefapSystem getPrefapSystem()
    {
        return m_prefapSystemComponent;
    }


    void OnCollisionEnter(Collision colision)
    {
        //if(colision.gameObject == Constants.getPlayer() && getStateComponent().canBeCoreToEnemy())
            //setToCoreEjector();
    }

    void OnDrawGizmos()
    {
        if(doStuff)
        {
            
            //doStuff = false;
        }
    }

    void doTestStuff()
    {
        setToCoreEjector();
    }


}
