using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGE : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_prefab;
    public GameObject m_cubeChildObject;
    public CgeTidyUp m_tidyUp;

    [Header("----- SETTINGS -----")]
    public int m_poolSize;
    public Vector3 m_inactivePosition;

    [Header("----- DEBUG -----")]
    public Queue<GameObject> m_inactiveCubes;
    public List<GameObject> m_activeCubes;

    [Header("------- Settings -------")]
    public float m_deactivateDistance;
    public int m_deactivisionsPerFrame;

    [Header("------- Debug -------")]
    public List<List<GameObject>> m_allCubes;
    public int m_checkedTotal;

   // public List<GameObject> m_activeCubes;
    public CGE m_cge;

    //public List<CgeInstance> m_instances;

    
    void Start()
    {
        if(m_cubeChildObject == null)
        {
            Debug.Log("Warning: No child object for cubes set!");
        }
        initializeInactiveCubes();
    }

    void FixedUpdate()
    {
        checkForDeactivate();
    }

    void initializeInactiveCubes()
    {
        m_inactiveCubes = new Queue<GameObject>();
        m_activeCubes = new List<GameObject>();

        for (int i = 0; i < m_poolSize; i++)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            GameObject cube = Instantiate(m_prefab, m_cubeChildObject.transform);
            cube.GetComponent<CubeEntitySystem>().addComponentsAtStart();
            cube.GetComponent<CubeEntitySystem>().setToInactive();
            //m_tidyUp.deactivateCube(cube);
            cube.transform.position = m_inactivePosition + Vector3.right * i * 4f;
            deactivateCube(cube);
        }
    }

    // Activate
    public void activateCube(Vector3 targetPosition)
    {
        if (m_inactiveCubes.Count > 0)
        {
            /*
            GameObject cube = null;
            do
            {
                cube = m_inactiveCubes.Dequeue();
                if(cube.GetComponent<CubeEntityState>().isInactive())
                {
                    m_activeCubes.Add(cube);
                    break;
                }

            } while (m_inactiveCubes.Count > 0);
            */
            GameObject cube = m_inactiveCubes.Dequeue();
            if (!cube.GetComponent<CubeEntityState>().isInactive())
            {
                m_activeCubes.Add(cube);
                if (m_inactiveCubes.Count > 0)
                {
                    cube = m_inactiveCubes.Dequeue();
                    if (!cube.GetComponent<CubeEntityState>().isInactive())
                        cube = null;
                }
            }

            if (cube != null)
            {

                cube.transform.position = targetPosition;
                //cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //cube.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                //cube.SetActive(true);
                //cube.transform.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 100f, ForceMode.Acceleration);
                //cube.transform.GetComponent<Rigidbody>().velocity = m_player.GetComponent<Rigidbody>().velocity * 0.5f;
                //cube.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

                m_activeCubes.Add(cube);
            }
        }
    }
    public void registerActiveCube(GameObject cube)
    {
        m_activeCubes.Add(cube);
        //m_inactiveCubes.Remove(cube);
        //m_inactiveCubes.Dequeue();
    }

    void checkForDeactivate()
    {
        int checkedThisRound = 0;
        //int checks = (int)Mathf.Min(m_movesAndScansPerFrame, Mathf.Max(m_cellQueue.Count * 0.1f, 5))
        while (checkedThisRound < m_deactivisionsPerFrame)
        {
            int index = m_activeCubes.Count - m_checkedTotal - 1;

            if (index < 0)
                index += m_activeCubes.Count;
            if (index < 0)
                index = 0;

            if (m_activeCubes.Count > 0)
            {
                GameObject cube = m_activeCubes[index];

                if (cube != null)
                {
                    bool constraint_0 = Vector3.Distance(m_player.transform.position, cube.transform.position) > m_deactivateDistance;
                    bool constraint_1 = cube.GetComponent<CubeEntitySystem>().getStateComponent().isInactive();

                    if (constraint_0 && constraint_1)
                    {
                        deactivateCube(cube);
                    }
                }
                else
                    Debug.Log("Warning: Cube was null");
            }

            checkedThisRound++;
            m_checkedTotal++;
            if (m_checkedTotal > m_activeCubes.Count)
                m_checkedTotal = 0;
        }
    }
    public void deactivateCube(GameObject cube)
    {
        //cube.SetActive(false);

        //Vector3 position = m_inactivePosition + Vector3.right * m_inactivePositionIndex * 4f;
        //m_inactivePositionIndex++;

        //cube.transform.position = position;
        //cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //cube.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //cube.GetComponent<CubeEntitySystem>().setToInactive();
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        m_activeCubes.Remove(cube);
        m_inactiveCubes.Enqueue(cube);
    }
}
