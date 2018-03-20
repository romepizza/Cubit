using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttachSystemBase : EntityCopiableAbstract
{
    public int m_maxCubesGrabbed;
    public List<GameObject> m_cubeList;
    public float m_movementAffectsCubesFactor;
    public abstract void deregisterCube(GameObject cube);
}
