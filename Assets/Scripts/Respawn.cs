using UnityEngine;

public class Respawn : MonoBehaviour
{
[Header("Referencia al punto respawn")]

    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && respawnPoint != null)
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if(controller !=null)
            {
                controller.enabled = false;
                other.transform.position = respawnPoint.position;
                controller.enabled = true;
            }
            else
            {
                other.transform.position = respawnPoint.position;
                Debug.Log("Jugador sin CharacterController fue movido al respawn");
            }
        }
    } // Start is called once before the first execution of Update after the MonoBehaviour is created

}
