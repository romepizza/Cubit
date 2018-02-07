using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEMBoidSystem : MonoBehaviour
{
    public GameObject m_object;

    [Header("------ Settings -------")]
    public int m_maxSwarmSize;
    public float m_maxSpeed;

    [Header("--- (Rules) ---")]
    public float m_resistancePower;
    public bool m_useLookInDirection;
    public bool m_useIsAffectedByPlayerMovement;

    [Header("- (Cohesion) -")]
    public bool m_useCohesion = true;
    public int m_cohesionPerFrame;
    public float m_cohesionPower;
    public float m_cohesionRadius;
    public int m_cohesionMaxPartners;
    public int m_cohesionMaxPartnerChecks;

    [Header("- (Alignment) -")]
    public bool m_useAlignment = true;
    public int m_alignmentPerFrame;
    public float m_alignmentPower;
    public float m_alignmentRadius;
    public int m_alignmentMaxPartners;
    public int m_alignmentMaxPartnerChecks;

    [Header("- (Seperation) -")]
    public bool m_useSeperation = true;
    public int m_separationPerFrame;
    public float m_separationPower;
    public float m_separationRadius;
    public int m_separationMaxPartners;
    public int m_separationMaxPartnerChecks;

    [Header("- (Repellence) -")]
    public bool m_useRepellence = true;
    //public List<float> m_repellencePowers;
    public float m_repellenceConstantPower;
    public float m_repellenceRadius;
    public int m_repellenceMaxPartners;
    public int m_repellenceMaxPartnerChecks;
    public GameObject[] m_repellenceObjects;
    public Material m_repellenceMaterial;


    [Header("- (Swarm Movement) -")]
    public bool m_useSwarmMovement = true;
    public GameObject m_swarmMovementObject;
    //public int m_swarmMovementPerFrame;
    public float m_swarmMovementPower;

    [Header("- (Gravitation)")]
    public bool m_useGravitation = true;
    public GameObject m_gravitationCenterObject;
    public int m_gravitationPerFrame;
    public float m_gravitationPower;
    //public bool m_gravitationToCenter;
    //public bool m_gravitationToObject;

    [Header("- (Line of Sight) -")]
    public bool m_useLineOfSight = true;

    [Header("- (Evasion) -")]
    public bool m_useEvasion = true;





    [Header("------- Debug -------")]
    public List<GameObject> m_agents;
    public Vector3 m_averageSwarmPosition;
    public Vector3 m_averageSwarmMovement;

    [Header("--- (Rules) ---")]

    [Header("- (Cohesion) -")]
    public int m_cohesionCounter;
    Dictionary<GameObject, Vector3> m_cohesionForceVectors;

    [Header("- (Alignment) -")]
    public int m_alignmentCounter;
    Dictionary<GameObject, Vector3> m_alignmentForceVectors;

    [Header("- (Seperation) -")]
    public int m_separationCounter;
    Dictionary<GameObject, Vector3> m_separationForceVectors;

    [Header("- (Repellence) -")]
    public Dictionary<GameObject, Vector3> m_repellenceForceVectors;

    [Header("- (Swarm Movement) -")]
    public int m_swarmMovementCounter;
    public Vector3 m_swarmMovementForceVector;

    [Header("- (Gravitation) -")]
    public int m_gravitationCounter;
    Dictionary<GameObject, Vector3> m_gravitationForceVectors;
    public Vector3 m_gravitationCenter;

    [Header("- (Line of Sight) -")]

    [Header("- (Evasion) -")]

    public bool b;
    void Start ()
    {
        initializeStuff();
	}

    void FixedUpdate()
    {
        if (m_agents.Count > 0)
        {
            applyBoidRules();
            applyPostMovement();
        }

        if(Input.GetKey(KeyCode.C))
        {
            foreach(GameObject agent in m_agents)
            {
                Vector3 v = agent.transform.position - m_averageSwarmPosition;
                agent.GetComponent<Rigidbody>().AddForce(v.normalized * 1000f, ForceMode.Acceleration);
            }
        }
    }

    void Update()
    {
        getInput();

        if (m_agents.Count > 0)
        {
            getStuff();
            getBoidInformation();
        }
    }

    void initializeStuff()
    {
        m_agents = new List<GameObject>();
        m_cohesionForceVectors = new Dictionary<GameObject, Vector3>();
        m_alignmentForceVectors = new Dictionary<GameObject, Vector3>();
        m_separationForceVectors = new Dictionary<GameObject, Vector3>();
        m_repellenceForceVectors = new Dictionary<GameObject, Vector3>();
        m_gravitationForceVectors = new Dictionary<GameObject, Vector3>();
    }

    void getStuff()
    {
        // average swarm position
        m_averageSwarmPosition = Vector3.zero;
        foreach(GameObject agent in m_agents)
        {
            m_averageSwarmPosition += agent.transform.position;
        }
        m_averageSwarmPosition /= m_agents.Count;
        

        // average swarm movement
        Vector3 averageMovementVector = Vector3.zero;
        foreach (GameObject agent2 in m_agents)
        {
            averageMovementVector += agent2.GetComponent<Rigidbody>().velocity;
        }
        averageMovementVector /= m_agents.Count;
        m_averageSwarmMovement = averageMovementVector;
        //m_averageSwarmMovement = GetComponent<Rigidbody>().velocity;
    }

    void applyBoidRules()
    {
        foreach (GameObject agent in m_agents)
        { 
            if (m_useCohesion)
                applyCohesion(agent);
            if (m_useAlignment)
                applyAlignment(agent);
            if (m_useSeperation)
                applySeparation(agent);
            if (m_useRepellence)
                applyRepellence(agent);
            if (m_useSwarmMovement && m_swarmMovementObject != null)
                applySwarmMovement(agent);
            if (m_useGravitation)
                applyGravitation(agent);
        }
        if (m_useLineOfSight)
            applyLineOfSight();
        if (m_useEvasion)
            applyEvasion();
    }
    void getBoidInformation()
    {
        if (m_useCohesion)
            manageCohesion();
        if (m_useAlignment)
            manageAlignment();
        if (m_useSeperation)
            manageSeparation();
        if (m_useRepellence)
            manageRepellence();
        if (m_useSwarmMovement && m_swarmMovementObject != null)
            manageSwarmMovement();
        if (m_useGravitation)
            manageGravitation();
        if (m_useLineOfSight)
            manageLineOfSight();
        if (m_useEvasion)
            manageEvasion();
    }
    void applyPostMovement()
    {
        foreach (GameObject agent in m_agents)
        {
            Rigidbody rb = agent.GetComponent<Rigidbody>();

            // (air) resistance
            if(rb.velocity.magnitude > 10f)
            {
                rb.AddForce(rb.velocity.normalized * -m_resistancePower);
            }
            // max speed
            rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude, m_maxSpeed);

            // look direction
            if(m_useLookInDirection && rb.velocity.magnitude > 0)
            {
                agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(rb.velocity), 0.3f);
            }
        }
    }

    // ----- Rules -----

    // Cohesion
    void manageCohesion()
    {
        manageCohesionCounter();
        /*
        foreach(GameObject agent in m_agents)
        {
            applyCohesion(agent);
        }
        */
    }
    void manageCohesionCounter()
    {
        int activisionsActually = m_cohesionPerFrame;
        if (m_cohesionPerFrame == 0)
            activisionsActually = m_agents.Count;
        if (m_cohesionPerFrame >= m_agents.Count)
            activisionsActually = m_agents.Count;

        for (int i = 0; i < activisionsActually; i++)
        {
            int index = m_cohesionCounter % m_agents.Count;
            getCohesionForceVector(m_agents[index]);
            m_cohesionCounter++;
        }
        m_cohesionCounter %= m_agents.Count;
    }
    void getCohesionForceVector(GameObject agent)
    {
        Vector3 localCenter = Vector3.zero;
        int nearAgentsCount = 0;
        int nearObjectsCount = 0;

        if (m_cohesionRadius <= 0)
        {
            foreach (GameObject agent2 in m_agents)
            {
                localCenter += agent2.transform.position;
                nearAgentsCount++;
            }

        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(agent.transform.position, m_cohesionRadius);
            foreach (Collider collider in colliders)
            {
                // max partner checks
                if (nearObjectsCount >= m_cohesionMaxPartnerChecks && m_cohesionMaxPartnerChecks > 0)
                    break;
                nearObjectsCount++;
                

                // max partners
                if (nearAgentsCount >= m_cohesionMaxPartners && m_cohesionMaxPartners > 0)
                    break;


                if (collider.GetComponent<CEMBoidAttached>() != null && collider.GetComponent<CEMBoidAttached>().m_attachedTo == this)
                {
                    localCenter += collider.gameObject.transform.position;

                    nearAgentsCount++;
                }
            }
        }
        if (nearAgentsCount > 0)
        {
            localCenter /= nearAgentsCount;
            m_cohesionForceVectors[agent] = (localCenter - agent.transform.position).normalized * m_cohesionPower;
        }
        else
        {
            m_cohesionForceVectors[agent] = Vector3.zero;
        }
    }
    void applyCohesion(GameObject agent)
    {
        agent.GetComponent<Rigidbody>().AddForce(m_cohesionForceVectors[agent], ForceMode.Acceleration);
    }
    // Alignment
    void manageAlignment()
    {
        manageAlignmentCounter();
        /*
        foreach (GameObject agent in m_agents)
        {
            applyAlignment(agent);
        }
        */
    }
    void manageAlignmentCounter()
    {
        int activisionsActually = m_alignmentPerFrame;
        if (m_alignmentPerFrame == 0)
            activisionsActually = m_agents.Count;
        if (m_alignmentPerFrame >= m_agents.Count)
            activisionsActually = m_agents.Count;

        for (int i = 0; i < activisionsActually; i++)
        {
            int index = m_alignmentCounter % m_agents.Count;
            getAlignmentForceVector(m_agents[index]);
            m_alignmentCounter++;
        }
        m_alignmentCounter %= m_agents.Count;
        
    }
    void getAlignmentForceVector(GameObject agent)
    {
        Vector3 localAverageMovementVector = Vector3.zero;
        int nearAgentsCount = 0;
        int nearObjectsCount = 0;

        Collider[] colliders = Physics.OverlapSphere(agent.transform.position, m_alignmentRadius);
        foreach (Collider collider in colliders)
        {
            // max partner checks
            if (nearObjectsCount >= m_alignmentMaxPartnerChecks && m_alignmentMaxPartnerChecks > 0)
                break;
            nearObjectsCount++;

            // max partners
            if (nearAgentsCount >= m_alignmentMaxPartners && m_alignmentMaxPartners > 0)
                break;

            if (collider.GetComponent<CEMBoidAttached>() != null && collider.GetComponent<CEMBoidAttached>().m_attachedTo == this)
            {
                //float distanceFactor = 1f - (Vector3.Distance(agent.transform.position, collider.transform.position) / m_alignmentRadius);
                if(collider.GetComponent<Rigidbody>() != null)
                    localAverageMovementVector += collider.GetComponent<Rigidbody>().velocity;// * distanceFactor;
                nearAgentsCount++;
            }
        }
        if (nearAgentsCount > 0)
        {
            localAverageMovementVector /= nearAgentsCount;
            m_alignmentForceVectors[agent] = localAverageMovementVector;
        }
        else
        {
            m_alignmentForceVectors[agent] = agent.GetComponent<Rigidbody>().velocity;
        }
    }
    void applyAlignment(GameObject agent)
    {
        Vector3 velocity = agent.GetComponent<Rigidbody>().velocity;
        
        agent.GetComponent<Rigidbody>().velocity = Vector3.Lerp(velocity, m_alignmentForceVectors[agent], m_alignmentPower);
    }
    // Separation
    void manageSeparation()
    {
        manageSeparationCounter();
        /*
        foreach (GameObject agent in m_agents)
        {
            applySeparation(agent);
        }*/
    }
    void manageSeparationCounter()
    {
        int activisionsActually = m_separationPerFrame;
        if (m_separationPerFrame == 0)
            activisionsActually = m_agents.Count;
        if (m_separationPerFrame >= m_agents.Count)
            activisionsActually = m_agents.Count;

        for (int i = 0; i < activisionsActually; i++)
        {
            int index = m_separationCounter % m_agents.Count;
            getSeparationForceVector(m_agents[index]);
            m_separationCounter++;
        }
        m_separationCounter %= m_agents.Count;
    }
    void getSeparationForceVector(GameObject agent)
    {
        Vector3 forceVector = Vector3.zero;
        int nearAgentsCount = 0;
        int nearObjectsCount = 0;


        Collider[] colliders = Physics.OverlapSphere(agent.transform.position, m_separationRadius);
        foreach (Collider collider in colliders)
        {
            // max partner checks
            if (nearObjectsCount >= m_separationMaxPartnerChecks && m_separationMaxPartnerChecks > 0)
                break;
            nearObjectsCount++;

            // max partners
            if (nearAgentsCount >= m_separationMaxPartners && m_separationMaxPartners > 0)
                break;

            if (collider.GetComponent<CEMBoidAttached>() != null && collider.GetComponent<CEMBoidAttached>().m_attachedTo == this)
            {
                float distanceFactor = Mathf.Clamp01(1f - (Vector3.Distance(agent.transform.position, collider.transform.position) / m_separationRadius));
                forceVector += (agent.transform.position - collider.gameObject.transform.position) * distanceFactor;
                nearAgentsCount++;
            }
        }
        m_separationForceVectors[agent] = forceVector.normalized * m_separationPower;
    }
    void applySeparation(GameObject agent)
    {
        agent.GetComponent<Rigidbody>().AddForce(m_separationForceVectors[agent], ForceMode.Acceleration);
    }
    // Repellence
    void manageRepellence()
    {
        manageRepellenceCounter();
    }
    void manageRepellenceCounter()
    {
        getRepellenceForceVector();
    }
    void getRepellenceForceVector()
    {
        foreach (GameObject agent in m_agents)
            m_repellenceForceVectors[agent] = Vector3.zero;

        foreach (GameObject repellor in m_repellenceObjects)
        {
            int nearAgentsCount = 0;
            int nearObjectsCount = 0;

            if (repellor == null)
                break;

            Collider[] colliders = Physics.OverlapSphere(repellor.transform.position, m_repellenceRadius);
            foreach (Collider collider in colliders)
            {
                GameObject agent = collider.gameObject;
                // max partner checks
                if (nearObjectsCount >= m_repellenceMaxPartnerChecks && m_repellenceMaxPartnerChecks > 0)
                    break;
                nearObjectsCount++;

                // max partners
                if (nearAgentsCount >= m_repellenceMaxPartners && m_repellenceMaxPartners > 0)
                    break;

                if (collider.GetComponent<CEMBoidAttached>() != null && collider.GetComponent<CEMBoidAttached>().m_attachedTo == this)
                {
                    float distanceFactor = Mathf.Clamp01(1f - (Vector3.Distance(agent.transform.position, repellor.transform.position) / m_repellenceRadius));
                    float directionFactor = Vector3.Dot((repellor.transform.position - agent.transform.position).normalized, agent.GetComponent<Rigidbody>().velocity.normalized);
                    //directionFactor = Mathf.Clamp01(directionFactor);
                    m_repellenceForceVectors[agent] += (agent.transform.position - repellor.gameObject.transform.position).normalized * m_repellenceConstantPower * distanceFactor;
                    nearAgentsCount++;
                }
            }
        }
    }
    void applyRepellence(GameObject agent)
    {
        agent.GetComponent<Rigidbody>().AddForce(m_repellenceForceVectors[agent], ForceMode.Acceleration);
    }
    // Swarm Movement
    void manageSwarmMovement()
    {
        //if (m_swarmMovementObject != null)
        {
            manageSwarmMovementCounter();
            /*
            foreach (GameObject agent in m_agents)
            {
                applySwarmMovement(agent);
            }
            */
        }
    }
    void manageSwarmMovementCounter()
    {
        getSwarmMovementForceVector();
    }
    void getSwarmMovementForceVector()
    {
        float distanceFactor = Mathf.Clamp01((Vector3.Distance(m_averageSwarmPosition, m_swarmMovementObject.transform.position) / 200f));
        m_swarmMovementForceVector = (m_swarmMovementObject.transform.position - m_averageSwarmPosition).normalized * m_swarmMovementPower * distanceFactor;
    }
    void applySwarmMovement(GameObject agent)
    {
        agent.GetComponent<Rigidbody>().AddForce(m_swarmMovementForceVector, ForceMode.Acceleration);
    }
    // Gravitation
    void manageGravitation()
    {
        manageGravitationCounter();
        /*
        foreach (GameObject agent in m_agents)
        {
            applyGravitation(agent);
        }
        */
    }
    void manageGravitationCounter()
    {
        int activisionsActually = m_gravitationPerFrame;
        if (m_gravitationPerFrame == 0)
            activisionsActually = m_agents.Count;
        if (m_gravitationPerFrame >= m_agents.Count)
            activisionsActually = m_agents.Count;

        if (m_gravitationCenterObject == null)
            m_gravitationCenter = m_averageSwarmPosition;
        else
            m_gravitationCenter = m_gravitationCenterObject.transform.position;

        for (int i = 0; i < activisionsActually; i++)
        {
            int index = m_gravitationCounter % m_agents.Count;
            getGravitationForceVector(m_agents[index]);
            m_gravitationCounter++;
        }
        m_gravitationCounter %= m_agents.Count;
    }
    void getGravitationForceVector(GameObject agent)
    {
        float distanceFactor = Mathf.Clamp01((Vector3.Distance(agent.transform.position, m_gravitationCenter) / 200f));
        m_gravitationForceVectors[agent] = (m_gravitationCenter - agent.transform.position).normalized * m_gravitationPower * distanceFactor;
    }
    void applyGravitation(GameObject agent)
    {
        agent.GetComponent<Rigidbody>().AddForce(m_gravitationForceVectors[agent], ForceMode.Acceleration);
    }

    
    // Line of Sight
    void manageLineOfSight()
    {

    }
    void applyLineOfSight()
    {

    }

    // Evasion
    void manageEvasion()
    {

    }
    void applyEvasion()
    {

    }
    

    public bool addAgent(GameObject agent)
    {
        if (m_agents.Count < m_maxSwarmSize)
        {
            if (agent != null)
            {
                if (agent.GetComponent < CEMBoidAttached>() == null)
                {
                    m_agents.Add(agent);
                    m_cohesionForceVectors.Add(agent, Vector3.zero);
                    m_alignmentForceVectors.Add(agent, Vector3.zero);
                    m_separationForceVectors.Add(agent, Vector3.zero);
                    m_repellenceForceVectors.Add(agent, Vector3.zero);
                    m_gravitationForceVectors.Add(agent, Vector3.zero);

                    CEMBoidAttached attachedScript = agent.AddComponent<CEMBoidAttached>();
                    attachedScript.m_attachedTo = this;

                    if(m_agents.Count - 1 < m_repellenceObjects.Length && m_repellenceObjects[m_agents.Count - 1] == null)
                    {
                        m_repellenceObjects[m_agents.Count - 1] = agent;
                        agent.GetComponent<Renderer>().material = m_repellenceMaterial;
                    }

                    return true;
                }
                else
                    Debug.Log("Warning: Tried to add agent that was already in the list!");
            }
        }
        return false;
    }
    public void removeAgent(GameObject agent)
    {
        if (m_agents.Count > 0)
        {
            if (agent != null)
            {
                if (m_agents.Contains(agent))
                {
                    m_agents.Remove(agent);
                    m_cohesionForceVectors.Remove(agent);
                    m_alignmentForceVectors.Remove(agent);
                    m_separationForceVectors.Remove(agent);
                    m_gravitationForceVectors.Remove(agent);
                }
                else
                    Debug.Log("Warning: Tried to add agent that was not in the list!");
            }
            else
            {
                m_agents.RemoveAt(Random.Range(0, m_agents.Count));
            }
        }
    }

    void getInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            addRandomCube();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            addAllCubes();
        }

        // Movement
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            m_useIsAffectedByPlayerMovement = !m_useIsAffectedByPlayerMovement;
        }
        // Grav power
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            m_gravitationPower = 25;
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            m_gravitationPower = 100;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            m_gravitationPower = 500;
        }
        // Repel force
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            m_repellenceRadius = 40;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            m_repellenceRadius = 100;
        }
        // Allignment
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            m_alignmentPower = 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            m_alignmentPower = 0.05f;
        }

    }
    void addRandomCube()
    {
        if (m_agents.Count < m_maxSwarmSize)
        {
            GameObject cubeAdd = null;
            Collider[] colliders = Physics.OverlapSphere(Constants.getPlayer().transform.position, 100);

            float nearestDist = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject cubePotential = colliders[i].gameObject;
                if (cubePotential.layer == 8 && cubePotential.GetComponent<CubeEntitySystem>() != null)
                {
                    if (cubePotential.GetComponent<CubeEntitySystem>().getStateComponent() != null && cubePotential.GetComponent<CubeEntitySystem>().getStateComponent().canBeAttachedToPlayer() && cubePotential.GetComponent<CEMBoidAttached>() == null)
                    {
                        if (true)
                        {
                            float dist = Vector3.Distance(Constants.getPlayer().transform.position, cubePotential.transform.position);
                            if (dist < nearestDist)
                            {
                                nearestDist = dist;
                                cubeAdd = cubePotential;
                            }
                        }
                        else
                        {
                            cubeAdd = cubePotential;
                            break;
                        }
                    }
                }
            }

            if (cubeAdd != null)
            {
                Constants.getPlayer().GetComponent<PlayerEntityAttachSystem>().addToGrab(cubeAdd, false, true);
                cubeAdd.GetComponent<CubeEntitySystem>().getPrefapSystem().setToAttachedPlayer();
                cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllAccelerationComponents();
                addAgent(cubeAdd);
                
            }
        }
    }

    void addAllCubes()
    {
        if (m_agents.Count < m_maxSwarmSize)
        {
            GameObject cubeAdd = null;
            Collider[] colliders = Physics.OverlapSphere(Constants.getPlayer().transform.position, 100);

            float nearestDist = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject cubePotential = colliders[i].gameObject;
                if (cubePotential.layer == 8 && cubePotential.GetComponent<CubeEntitySystem>() != null)
                {
                    if (cubePotential.GetComponent<CubeEntitySystem>().getStateComponent() != null && cubePotential.GetComponent<CubeEntitySystem>().getStateComponent().canBeAttachedToPlayer())
                    {
                        cubeAdd = cubePotential;
                        if (addAgent(cubeAdd))
                        { 
                            Constants.getPlayer().GetComponent<PlayerEntityAttachSystem>().addToGrab(cubeAdd, false, true);
                            cubeAdd.GetComponent<CubeEntitySystem>().getPrefapSystem().setToAttachedPlayer();
                            cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllAccelerationComponents();
                        }
                    }
                }
            }

            /*
            if (cubeAdd != null)
            {
                Constants.getPlayer().GetComponent<PlayerEntityAttachSystem>().addToGrab(cubeAdd, false, true);
                cubeAdd.GetComponent<CubeEntitySystem>().getPrefapSystem().setToAttachedPlayer();
                cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllAccelerationComponents();
                addAgent(cubeAdd);
            }*/
        }
    }

    void removeRandomCube()
    {

    }
}
