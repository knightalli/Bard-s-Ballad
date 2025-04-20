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
        }

        if (count == 1)
        {
            ChangeRoom roomChangerLToD = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToD.playerChangePos = new Vector3(0f, -2f);

            roomChangerLToD.limitChangeBottom = -14;
            roomChangerLToD.limitChangeUpper = -14;
            roomChangerLToD.limitChangeLeft = 0;
            roomChangerLToD.limitChangeRight = 0;

            ChangeRoom roomChangerUToL = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToL.playerChangePos = new Vector3(-2f, 0f);

            roomChangerUToL.limitChangeBottom = 0;
            roomChangerUToL.limitChangeUpper = 0;
            roomChangerUToL.limitChangeLeft = -14;
            roomChangerUToL.limitChangeRight = -14;

            ChangeRoom roomChangerRToU = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToU.playerChangePos = new Vector3(0f, 2f);

            roomChangerRToU.limitChangeBottom = 14;
            roomChangerRToU.limitChangeUpper = 14;
            roomChangerRToU.limitChangeLeft = 0;
            roomChangerRToU.limitChangeRight = 0;

            ChangeRoom roomChangerDToR = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToR.playerChangePos = new Vector3(2f, 0f);

            roomChangerDToR.limitChangeBottom = 0;
            roomChangerDToR.limitChangeUpper = 0;
            roomChangerDToR.limitChangeLeft = 14;
            roomChangerDToR.limitChangeRight = 14;
        }
        else if (count == 2)
        {
            ChangeRoom roomChangerLToR = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToR.playerChangePos = new Vector3(2f, 0f);

            roomChangerLToR.limitChangeBottom = 0;
            roomChangerLToR.limitChangeUpper = 0;
            roomChangerLToR.limitChangeLeft = 14;
            roomChangerLToR.limitChangeRight = 14;

            ChangeRoom roomChangerUToD = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToD.playerChangePos = new Vector3(0f, -2f);

            roomChangerUToD.limitChangeBottom = -14;
            roomChangerUToD.limitChangeUpper = -14;
            roomChangerUToD.limitChangeLeft = 0;
            roomChangerUToD.limitChangeRight = 0;

            ChangeRoom roomChangerRToL = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToL.playerChangePos = new Vector3(-2f, 0f);

            roomChangerRToL.limitChangeBottom = 0;
            roomChangerRToL.limitChangeUpper = 0;
            roomChangerRToL.limitChangeLeft = -14;
            roomChangerRToL.limitChangeRight = -14;

            ChangeRoom roomChangerDToU = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToU.playerChangePos = new Vector3(0f, 2f);

            roomChangerDToU.limitChangeBottom = 14;
            roomChangerDToU.limitChangeUpper = 14;
            roomChangerDToU.limitChangeLeft = 0;
            roomChangerDToU.limitChangeRight = 0;
        }
        else if (count == 3)
        {
            ChangeRoom roomChangerLToU = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToU.playerChangePos = new Vector3(0f, 2f);

            roomChangerLToU.limitChangeBottom = 14;
            roomChangerLToU.limitChangeUpper = 14;
            roomChangerLToU.limitChangeLeft = 0;
            roomChangerLToU.limitChangeRight = 0;

            ChangeRoom roomChangerUToR = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToR.playerChangePos = new Vector3(2f, 0f);

            roomChangerUToR.limitChangeBottom = 0;
            roomChangerUToR.limitChangeUpper = 0;
            roomChangerUToR.limitChangeLeft = 14;
            roomChangerUToR.limitChangeRight = 14;

            ChangeRoom roomChangerRToD = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToD.playerChangePos = new Vector3(0f, -2f);

            roomChangerRToD.limitChangeBottom = -14;
            roomChangerRToD.limitChangeUpper = -14;
            roomChangerRToD.limitChangeLeft = 0;
            roomChangerRToD.limitChangeRight = 0;

            ChangeRoom roomChangerDToL = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToL.playerChangePos = new Vector3(-2f, 0f);

            roomChangerDToL.limitChangeBottom = 0;
            roomChangerDToL.limitChangeUpper = 0;
            roomChangerDToL.limitChangeLeft = -14;
            roomChangerDToL.limitChangeRight = -14;
        }
    }
}
