using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; 

public class DroneObjectPool : MonoBehaviour
{
    public int maxPoolSize = 10;
    public int stackDefaultCapacity = 10;

        public IObjectPool<Drone> Pool
        {
            get
            {
                if (_pool == null)
                    _pool =
                        new ObjectPool<Drone>(
                            CreatedPooledItem,
                            OnTakeFromPool,
                            OnReturnedToPool,
                            OnDestroyPoolObject,
                            true,
                            stackDefaultCapacity,
                            maxPoolSize);
                return _pool;
            }
        }

        private IObjectPool<Drone> _pool;

        //We are initializing our drone instances
        private Drone CreatedPooledItem()
        {
            var go =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            Drone drone = go.AddComponent<Drone>();

            go.name = "Drone";
            drone.Pool = Pool;

            return drone;
        }

        //We are not destroying the drone gameObject instead we 
        //  are deactivating it to remove it from the scene
        private void OnReturnedToPool(Drone drone)
        {
            drone.gameObject.SetActive(false);
        }
        
        // This is called when the client requests an instance of the 
        //  drone.
        //The instance is not actually returned but reactivated in the 
        //  scene. 
        private void OnTakeFromPool(Drone drone)
        {
            drone.gameObject.SetActive(true);
        }

        //This method is called when there is no more space in the 
        //  pool.
        //The returned instance is destroyed to free up memory.
        private void OnDestroyPoolObject(Drone drone)
        {
            Destroy(drone.gameObject);
        }

        public void Spawn()
        {
            var amount = Random.Range(1, 10);

            for (int i = 0; i < amount; ++i)
            {
                var drone = Pool.Get();

                drone.transform.position =
                    Random.insideUnitSphere * 10;
            }
        }
 }