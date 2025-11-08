using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] Color32 wthoPacketColor = new Color32(0, 0, 255, 255);
    [SerializeField] Color32 wthPacketColor = new Color32(255, 255, 255, 255);
    bool hasPacket = false;
    
    private void OnCollisionEnter2D(UnityEngine.Collision2D other) {
        Debug.Log("Collision");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Packet" && !hasPacket) 
        {
            Debug.Log("Packet picked up");
            this.gameObject.GetComponent<SpriteRenderer>().color = wthPacketColor;
            hasPacket = true; 
            Destroy(other.gameObject, 0.3f);

        } else if(other.gameObject.tag == "Customer" && hasPacket) {
                Debug.Log("Customer served");
                this.gameObject.GetComponent<SpriteRenderer>().color = wthoPacketColor;
                hasPacket = false;
        }
    }
}