using UnityEngine;

namespace Objects
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        public MeshRenderer MeshRenderer => meshRenderer;
    }
}