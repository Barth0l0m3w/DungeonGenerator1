using UnityEngine;
using Unity.AI.Navigation;
using NaughtyAttributes;

public class NavMeshBuild : MonoBehaviour
{
    public NavMeshSurface navService;
    
    [Button]
    public void BuildNavigation()
    {
        navService.RemoveData();//clear the old bake
        navService.BuildNavMesh();//make a new bake according to the new dungeon
    }
    
    public void ClearOldNav()
    {
        navService.RemoveData();
    }
}
