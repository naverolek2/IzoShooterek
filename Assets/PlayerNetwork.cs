using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{
    Transform spawnedObjectTransform;


    [SerializeField] private Transform spawnedObjectPrefab;


    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
    new MyCustomData
    {
        _int = 56,
        _bool = true,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    };


    private void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    




    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            spawnedObjectTransform.parent = null;
            TestClientRPC(new ClientRpcParams { Send = new ClientRpcSendParams {  TargetClientIds = new List<ulong> { 1 } } });
            /*
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "You're gay",
            };
            */
        }
        if(Input.GetKeyDown(KeyCode.Y)) {
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectTransform.gameObject);
        }
    }
    
    [ServerRpc]
    private void TestServerRPC(ServerRpcParams serverRpcParams)
    {
        
    }
    [ClientRpc]
    private void TestClientRPC(ClientRpcParams clientRpcParams) { 
    
    }




}
