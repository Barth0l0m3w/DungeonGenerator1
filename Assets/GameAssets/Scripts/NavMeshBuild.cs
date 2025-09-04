using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using Unity.AI;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class NavMeshBuild : MonoBehaviour
{
    [FormerlySerializedAs("navSurvice")] public NavMeshSurface navService;
    
    [Button]
    public void BuildNavigation()
    {
        navService.RemoveData();//clear the old bake
        navService.BuildNavMesh();//make a new bake according to the new dungeon
    }
}
