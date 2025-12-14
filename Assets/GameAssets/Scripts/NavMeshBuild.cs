using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;

namespace GameAssets.Scripts
{
    public class NavMeshBuild : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navSurface;
    
        [Button]
        public void BuildNavigation()
        {
            _navSurface.RemoveData();//clear the old bake
            _navSurface.BuildNavMesh();//make a new bake according to the new dungeon
        }
    
        public void ClearOldNav()
        {
            _navSurface.RemoveData();
        }
    }
}
