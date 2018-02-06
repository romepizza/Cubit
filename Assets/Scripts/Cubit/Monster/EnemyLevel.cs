using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    [Header("--- (Ejector) ---")]
    [Header("- (Hp Ejector) -")]
    public int[] ejectorHps;
    public int[] ejectorMaxCubes;

    [Header("- (Form Ejector) -")]
    public float[] ejectorMaxRadiusCircles;
    public float[] ejectorMinRadiusCircles;

    [Header("- (Grab Ejector) -")]
    public float[] ejectorGrabCooldowns;
    public float[] ejectorGrabLessCooldownPerCubes;
    public float[] ejectorGrabMinCooldowns;

    [Header("- (Shoot Ejector) -")]
    public float[] ejectorShotCooldowns;
    public float[] ejectorShotMaxSpeeds;
    public float[] ejectorShotLessCooldownPerCubes;
    public float[] ejectorShotMinCooldowns;

    [Header("- (Shoot Help Ejector")]
    public float[] ejectorShotInMoveDirectionMinFactor;
    public float[] ejectorShotInMoveDirectionMaxFactor;


    [Header("--- (Worm) ---")]
    public int[] wormHps;
    public int[] wormMaxCubes;

    [Header("- (Form Worm) -")]
    public float[] wormMaxRadiusCircles;
    public float[] wormMinRadiusCircles;

    [Header("- (Grab Worm) -")]
    public float[] wormGrabCooldowns;
    public float[] wormGrabLessCooldownPerCubes;
    public float[] wormGrabMinCooldowns;

    [Header("- (Movement Worm) -")]
    public float[] wormMovementMaxSpeed;
    public float[] wormMovementPower;
    public float[] wormMovementMinAngle;
    public float[] wormMovementDeviationFactor;
    public float[] wormMovementOffsetRadius;


	

    public void setEnemyLevel(int currentLevel)
    {
        foreach(Transform cubeTransform in transform)
        {
            GameObject cube = cubeTransform.gameObject;
            if(cube.GetComponent<ColorCube>() != null)
            {
                CubeMonster es = cube.GetComponent<CubeMonster>();

                if (currentLevel < ejectorHps.Length && ejectorHps[currentLevel] >= 0)
                    es.maxLife = ejectorHps[currentLevel];
                if (currentLevel < ejectorMaxCubes.Length && ejectorMaxCubes[currentLevel] >= 0)
                    es.maxCubes = ejectorMaxCubes[currentLevel];

                if (currentLevel < ejectorMaxRadiusCircles.Length && ejectorMaxRadiusCircles[currentLevel] >= 0)
                    es.radiusCircle = ejectorMaxRadiusCircles[currentLevel];
                if (currentLevel < ejectorMinRadiusCircles.Length && ejectorMinRadiusCircles[currentLevel] >= 0)
                    es.minRadiusCircle = ejectorMinRadiusCircles[currentLevel];

                if (currentLevel < ejectorGrabCooldowns.Length && ejectorGrabCooldowns[currentLevel] >= 0)
                    es.grabCooldown = ejectorGrabCooldowns[currentLevel];
                if (currentLevel < ejectorGrabLessCooldownPerCubes.Length && ejectorGrabLessCooldownPerCubes[currentLevel] >= 0)
                    es.grabLessCooldownPerAttachedCube = ejectorGrabLessCooldownPerCubes[currentLevel];
                if (currentLevel < ejectorGrabMinCooldowns.Length && ejectorGrabMinCooldowns[currentLevel] >= 0)
                    es.minCooldownGrab = ejectorGrabMinCooldowns[currentLevel];

                if (currentLevel < ejectorShotCooldowns.Length && ejectorShotCooldowns[currentLevel] >= 0)
                    es.shootCooldown = ejectorShotCooldowns[currentLevel];
                if (currentLevel < ejectorShotMaxSpeeds.Length && ejectorShotMaxSpeeds[currentLevel] >= 0)
                    es.shootMaxSpeed = ejectorShotMaxSpeeds[currentLevel];
                if (currentLevel < ejectorShotLessCooldownPerCubes.Length && ejectorShotLessCooldownPerCubes[currentLevel] >= 0)
                    es.shootLessCooldownPerAttachedCube = ejectorShotLessCooldownPerCubes[currentLevel];
                if (currentLevel < ejectorShotMinCooldowns.Length && ejectorShotMinCooldowns[currentLevel] >= 0)
                    es.minCooldownShoot = ejectorShotMinCooldowns[currentLevel];

                if (currentLevel < ejectorShotInMoveDirectionMinFactor.Length && ejectorShotInMoveDirectionMinFactor[currentLevel] >= 0)
                    es.shootInPlayerMoveDirectionMinRandom = ejectorShotInMoveDirectionMinFactor[currentLevel];
                if (currentLevel < ejectorShotInMoveDirectionMaxFactor.Length && ejectorShotInMoveDirectionMaxFactor[currentLevel] >= 0)
                    es.shootInPlayerMoveDirectionMaxRandom = ejectorShotInMoveDirectionMaxFactor[currentLevel];
                

                MonsterChase ms = cube.GetComponent<MonsterChase>();

                if (currentLevel < wormHps.Length && wormHps[currentLevel] >= 0)
                    ms.maxLife = wormHps[currentLevel];
                if (currentLevel < wormMaxCubes.Length && wormMaxCubes[currentLevel] >= 0)
                    ms.maxCubes = wormMaxCubes[currentLevel];

                if (currentLevel < wormMaxRadiusCircles.Length && wormMaxRadiusCircles[currentLevel] >= 0)
                    ms.radiusCircle = wormMaxRadiusCircles[currentLevel];
                if (currentLevel < wormMinRadiusCircles.Length && wormMinRadiusCircles[currentLevel] >= 0)
                    ms.minRadiusCircle = wormMinRadiusCircles[currentLevel];

                if (currentLevel < wormGrabCooldowns.Length && wormGrabCooldowns[currentLevel] >= 0)
                    ms.grabCooldown = wormGrabCooldowns[currentLevel];
                if (currentLevel < wormGrabLessCooldownPerCubes.Length && wormGrabLessCooldownPerCubes[currentLevel] >= 0)
                    ms.grabLessCooldownPerAttachedCube = wormGrabLessCooldownPerCubes[currentLevel];
                if (currentLevel < wormGrabMinCooldowns.Length && wormGrabMinCooldowns[currentLevel] >= 0)
                    ms.minCooldownGrab = wormGrabMinCooldowns[currentLevel];
                
                if (currentLevel < wormMovementMaxSpeed.Length && wormMovementMaxSpeed[currentLevel] >= 0)
                    ms.movementMaxSpeed = wormMovementMaxSpeed[currentLevel];
                if (currentLevel < wormMovementPower.Length && wormMovementPower[currentLevel] >= 0)
                    ms.movementMovePower = wormMovementPower[currentLevel];
                if (currentLevel < wormMovementMinAngle.Length && wormMovementMinAngle[currentLevel] >= 0)
                    ms.minAngleMovement = wormMovementMinAngle[currentLevel];
                if (currentLevel < wormMovementDeviationFactor.Length && wormMovementDeviationFactor[currentLevel] >= 0)
                    ms.deviationPower = wormMovementDeviationFactor[currentLevel];
                if (currentLevel < wormMovementOffsetRadius.Length && wormMovementOffsetRadius[currentLevel] >= 0)
                    ms.movementOffsetRadius = wormMovementOffsetRadius[currentLevel];
            }
        }
    }
}
