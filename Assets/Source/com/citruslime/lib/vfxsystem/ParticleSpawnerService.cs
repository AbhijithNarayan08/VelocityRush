using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using com.citruslime.game.vfx;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.assetmanagement;

namespace com.citruslime.lib.vfxsystem
{
    public class ParticleSpawnerService 
    {
        [SerializeField] private  List<AssetReference> particleReferences;
        [SerializeField] private List<VfxNameEnums> VfxNameEnumsRef;
        private readonly Dictionary<AssetReference, List<GameObject>> spawnedParticleSystems = new Dictionary<AssetReference, List<GameObject>>();
        private readonly Dictionary<AssetReference, Queue<Vector3>> queuedSpawnRequests = new Dictionary<AssetReference, Queue<Vector3>>();
        private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> asyncOperationHandle = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
        
        private AssetFactory assetFactory = null;
        public static event Action<AssetReference> InstantiatedEffect;
    
        ///<summary> Spawns a VFX at position under Parent Transform </summary>
        ///<param> _name = VfxNameEnums </param>
        ///<param> spawnPosition = Vector3 Position </param>
        ///<param> parent = Parent Transform </param>


        public ParticleSpawnerService()
        {
            DependencyInjector.Instance.InjectDependencies(this);
            init();
        }

        private void init()
        {
            if (assetFactory != null)
            {

                ParticleRefData refData = Resources.Load<ParticleRefData>("VFX/ParticleRefSo");
              
                particleReferences = refData.particleReferences;
                VfxNameEnumsRef = refData.vfxNameEnums;
            }
        }

        [Inject]
        private void inject(AssetFactory _asm)
        {
            assetFactory = _asm;
        }
        internal void Spawn(VfxNameEnums _name, Vector3 spawnPosition, Transform parent)
        {
            AssetReference assetReference = null;
            string name = _name.ToString();
            int a = VfxNameEnumsRef.IndexOf(_name);
            if(String.IsNullOrWhiteSpace(name)|| String.IsNullOrEmpty(name))
            {
                return;
            }
            Debug.Log("Addressable name " + name);


            assetReference = particleReferences[a];                         // (O) n^2 such complexity much wow , make it a hasmap -Abhijith
            // foreach (var item in particleReferences)
            // {
            //     // if(item.editorAsset.name == name)
            //     // {
            //     //     assetReference = item;
            //     // }
            //     // else
            //     // {
            //     //     Debug.Log("MisMatch particle name");                    ///very noob way of doing but it works remind me to rework on this system = abhijith
            //     // }
            // }
            
            if(!assetReference.RuntimeKeyIsValid())
            {
                Debug.Log("Invalid Key "+ assetReference.RuntimeKey.ToString());
                return;
            }

            if(asyncOperationHandle.ContainsKey(assetReference))
            {
                if(asyncOperationHandle[assetReference].IsDone)
                {
                    SpawnParticleFromLoadedReference(assetReference, spawnPosition, parent);
                }
                else
                {
                    EnqueueSpawnForAfterInitialization(assetReference);
                }
                return;
            }

            LoadAndSpawn(assetReference, spawnPosition, parent);
        }
        
        void LoadAndSpawn(AssetReference assetReference, Vector3 position, Transform parent)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
            asyncOperationHandle[assetReference] = op;
            op.Completed += (operation) => {
                SpawnParticleFromLoadedReference(assetReference, position, parent);
                if(queuedSpawnRequests.ContainsKey(assetReference))
                {
                    while(queuedSpawnRequests[assetReference]?.Any() == true)
                    {
                        var position = queuedSpawnRequests[assetReference].Dequeue();
                        SpawnParticleFromLoadedReference(assetReference, position, parent);
                    }
                }
            };
        }
        Vector3 GetRandomPosition()
        {
            return new Vector3(UnityEngine.Random.Range(-5,5),1, UnityEngine.Random.Range(-5,5));    
        }
        void SpawnParticleFromLoadedReference(AssetReference assetReference, Vector3 position, Transform parent)
        {
            assetReference.InstantiateAsync(position, Quaternion.identity).Completed += (asyncOperationHandles) =>{
                if(!spawnedParticleSystems.ContainsKey(assetReference))
                {
                    spawnedParticleSystems[assetReference] = new List<GameObject>();
                }
                spawnedParticleSystems[assetReference].Add(asyncOperationHandles.Result);
                asyncOperationHandles.Result.transform.SetParent(parent);
                // if(parent != this.gameObject.transform)
                // {
                //     ResetVFXPosition(asyncOperationHandles.Result.transform);
                // }
                var notify = asyncOperationHandles.Result.AddComponent<NotifyOnDestroy>();
                notify.Destoryed += Remove;
                notify.assetReference = assetReference;
            };
            InstantiatedEffect?.Invoke(assetReference);
        }
        

        void EnqueueSpawnForAfterInitialization(AssetReference assetReference)
        {
            if(!queuedSpawnRequests.ContainsKey(assetReference))
            {
                queuedSpawnRequests[assetReference] = new Queue<Vector3>();
            }
            queuedSpawnRequests[assetReference].Enqueue(GetRandomPosition());
        }
        public void Remove(AssetReference assetReference, NotifyOnDestroy obj)
        {
            Addressables.ReleaseInstance(obj.gameObject);

            spawnedParticleSystems[assetReference].Remove(obj.gameObject);
            if(spawnedParticleSystems[assetReference].Count == 0)
            {
                if(asyncOperationHandle[assetReference].IsValid())
                {
                    Addressables.Release(asyncOperationHandle[assetReference]);
                }
                if(asyncOperationHandle[assetReference].IsValid())
                {
                    asyncOperationHandle.Remove(assetReference);
                }
            }
        }

        // public void SpawnParticlesRandom(VfxNameEnums _name , int spawnCount = 1)
        // {
        //     for(int i = 0; i< spawnCount; i++)
        //     {
        //         Spawn(_name, GetRandomPosition(), gameObject.transform);
        //     }
        // }
        
        public void SpawnParticleAtPositionInParent(VfxNameEnums _name, int spawnCount, Vector3 position, Transform parent)
        {
            for(int i = 0; i< spawnCount; i++)
            {
                Spawn(_name, position, parent);
            }
        }
        
        // public void SpawnParticleAtPosition(VfxNameEnums _name, int spawnCount, Vector3 position)
        // {
        //     for(int i = 0; i< spawnCount; i++)
        //     {
        //         Spawn(_name, position, gameObject.transform);
        //     }
        // }
        
        void ResetVFXPosition(Transform obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }

}
