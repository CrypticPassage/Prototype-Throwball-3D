using UnityEngine;

namespace Signals
{
    public class SignalObjectsCollision
    {
        public GameObject ObjectThatEnteredCollision;
        public Collision Collision;
        
        public SignalObjectsCollision(GameObject objectThatEnteredCollision, Collision collision)
        {
            ObjectThatEnteredCollision = objectThatEnteredCollision;
            Collision = collision;
        }
    }
}