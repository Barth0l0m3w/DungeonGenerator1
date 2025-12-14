using UnityEngine;

namespace GameAssets.Scripts
{
    public class RoomBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject[] _walls; // 0-up 1-down 2-right 3-left
        [SerializeField] private GameObject[] _doors;
    
        public void UpdateRoom(bool[] status)
        {
            for (int i = 0; i < status.Length; i++)
            {
                _doors[i].SetActive(status[i]);
                _walls[i].SetActive(!status[i]);
            }
        }
    }
}
