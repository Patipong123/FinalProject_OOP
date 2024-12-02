using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // ตัวแปรอ้างอิงถึงผู้เล่น
    public float speed; // ความเร็วในการเคลื่อนที่ของกล้อง (ไม่จำเป็นในกรณีนี้)
    public float clampLeft; // ขอบซ้ายที่กล้องจะหยุด
    public float clampRight; // ขอบขวาที่กล้องจะหยุด

    private float offsetX; // ระยะห่างระหว่างกล้องและผู้เล่น

    // Start is called before the first frame update
    void Start()
    {
        // คำนวณระยะห่างระหว่างกล้องกับผู้เล่นตอนเริ่มเกม
        offsetX = transform.position.x - player.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // ตรวจสอบว่าผู้เล่นไม่ใช่ null
        {
            // ตำแหน่งใหม่ของกล้องคือ ตำแหน่งของผู้เล่น + offset
            float targetX = player.position.x + offsetX;

            // จำกัดการเคลื่อนไหวของกล้องให้อยู่ในขอบเขต
            targetX = Mathf.Clamp(targetX, clampLeft, clampRight);

            // อัปเดตตำแหน่งกล้อง
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }
}
