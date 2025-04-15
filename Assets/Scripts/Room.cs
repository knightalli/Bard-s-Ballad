using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject DoorU;
    public GameObject DoorD;
    public GameObject DoorL;
    public GameObject DoorR;
    
    public GameObject RoomMoverU;
    public GameObject RoomMoverD;
    public GameObject RoomMoverL;
    public GameObject RoomMoverR;
    void Start()
    {
        
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 0, 90);

            GameObject tmp = DoorL;
            DoorL = DoorU;
            DoorU = DoorR;
            DoorR = DoorD;
            DoorD = tmp;

            ChangeRoom roomChangerLToU = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToU.playerChangePos = new Vector3(0f, 2f);
            if (roomChangerLToU.limitChangeBottom < 14)
            {
                roomChangerLToU.limitChangeBottom += 14;
            }
            else
            {
                roomChangerLToU.limitChangeBottom = -14;
            }
            if (roomChangerLToU.limitChangeUpper < 14)
            {
                roomChangerLToU.limitChangeUpper += 14;
            }
            else
            {
                roomChangerLToU.limitChangeUpper = -14;
            }
            if (roomChangerLToU.limitChangeLeft < 14)
            {
                roomChangerLToU.limitChangeLeft += 14;
            }
            else
            {
                roomChangerLToU.limitChangeLeft = -14;
            }
            if (roomChangerLToU.limitChangeRight < 14)
            {
                roomChangerLToU.limitChangeRight += 14;
            }
            else
            {
                roomChangerLToU.limitChangeRight = -14;
            }
            
            ChangeRoom roomChangerUToR = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToR.playerChangePos = new Vector3(2f, 0f);
            if (roomChangerUToR.limitChangeBottom < 14)
            {
                roomChangerUToR.limitChangeBottom += 14;
            }
            else
            {
                roomChangerUToR.limitChangeBottom = -14;
            }
            if (roomChangerUToR.limitChangeUpper < 14)
            {
                roomChangerUToR.limitChangeUpper += 14;
            }
            else
            {
                roomChangerUToR.limitChangeUpper = -14;
            }
            if (roomChangerUToR.limitChangeLeft < 14)
            {
                roomChangerUToR.limitChangeLeft += 14;
            }
            else
            {
                roomChangerUToR.limitChangeLeft = -14;
            }
            if (roomChangerUToR.limitChangeRight < 14)
            {
                roomChangerUToR.limitChangeRight += 14;
            }
            else
            {
                roomChangerUToR.limitChangeRight = -14;
            }
            
            ChangeRoom roomChangerRToD = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToD.playerChangePos = new Vector3(0f, -2f);
            if (roomChangerRToD.limitChangeBottom < 14)
            {
                roomChangerRToD.limitChangeBottom += 14;
            }
            else
            {
                roomChangerRToD.limitChangeBottom = -14;
            }
            if (roomChangerRToD.limitChangeUpper < 14)
            {
                roomChangerRToD.limitChangeUpper += 14;
            }
            else
            {
                roomChangerRToD.limitChangeUpper = -14;
            }
            if (roomChangerRToD.limitChangeLeft < 14)
            {
                roomChangerRToD.limitChangeLeft += 14;
            }
            else
            {
                roomChangerRToD.limitChangeLeft = -14;
            }
            if (roomChangerRToD.limitChangeRight < 14)
            {
                roomChangerRToD.limitChangeRight += 14;
            }
            else
            {
                roomChangerRToD.limitChangeRight = -14;
            }
            
            ChangeRoom roomChangerDToL = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToL.playerChangePos = new Vector3(-2f, 0f);
            if (roomChangerDToL.limitChangeBottom < 14)
            {
                roomChangerDToL.limitChangeBottom += 14;
            }
            else
            {
                roomChangerDToL.limitChangeBottom = -14;
            }
            if (roomChangerDToL.limitChangeUpper < 14)
            {
                roomChangerDToL.limitChangeUpper += 14;
            }
            else
            {
                roomChangerDToL.limitChangeUpper = -14;
            }
            if (roomChangerDToL.limitChangeLeft < 14)
            {
                roomChangerDToL.limitChangeLeft += 14;
            }
            else
            {
                roomChangerDToL.limitChangeLeft = -14;
            }
            if (roomChangerDToL.limitChangeRight < 14)
            {
                roomChangerDToL.limitChangeRight += 14;
            }
            else
            {
                roomChangerDToL.limitChangeRight = -14;
            }
        }
    }
}
