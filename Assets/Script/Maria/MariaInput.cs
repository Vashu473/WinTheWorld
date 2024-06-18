using Unity.Netcode;
using UnityEngine;

public class MariaInput : NetworkBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    public NetworkVariable<Vector3> spawnLocation = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (!IsLocalPlayer) return;

        transform.position = spawnLocation.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            SendMovementInput(); // Send input to server
        }
    }

    private void SendMovementInput()
    {
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);

        Vector2 movement = new Vector2(left ? -1 : (right ? 1 : 0), up ? -1 : (down ? 1 : 0));
        HandleMovementInputServerRpc(movement); // Send input as an RPC
    }

    [ServerRpc]
    void HandleMovementInputServerRpc(Vector2 movement)
    {
        Vector3 moveVector = new Vector3(movement.x, 0, movement.y);
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}

