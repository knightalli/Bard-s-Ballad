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
            roomChangerLToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerLToD.limitChangeBottom = -22;
            roomChangerLToD.limitChangeUpper = -22;
            roomChangerLToD.limitChangeLeft = 0;
            roomChangerLToD.limitChangeRight = 0;

            ChangeRoom roomChangerUToL = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerUToL.limitChangeBottom = 0;
            roomChangerUToL.limitChangeUpper = 0;
            roomChangerUToL.limitChangeLeft = -22;
            roomChangerUToL.limitChangeRight = -22;

            ChangeRoom roomChangerRToU = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerRToU.limitChangeBottom = 22;
            roomChangerRToU.limitChangeUpper = 22;
            roomChangerRToU.limitChangeLeft = 0;
            roomChangerRToU.limitChangeRight = 0;

            ChangeRoom roomChangerDToR = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerDToR.limitChangeBottom = 0;
            roomChangerDToR.limitChangeUpper = 0;
            roomChangerDToR.limitChangeLeft = 22;
            roomChangerDToR.limitChangeRight = 22;
        }
        else if (count == 2)
        {
            ChangeRoom roomChangerLToR = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerLToR.limitChangeBottom = 0;
            roomChangerLToR.limitChangeUpper = 0;
            roomChangerLToR.limitChangeLeft = 22;
            roomChangerLToR.limitChangeRight = 22;

            ChangeRoom roomChangerUToD = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerUToD.limitChangeBottom = -22;
            roomChangerUToD.limitChangeUpper = -22;
            roomChangerUToD.limitChangeLeft = 0;
            roomChangerUToD.limitChangeRight = 0;

            ChangeRoom roomChangerRToL = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerRToL.limitChangeBottom = 0;
            roomChangerRToL.limitChangeUpper = 0;
            roomChangerRToL.limitChangeLeft = -22;
            roomChangerRToL.limitChangeRight = -22;

            ChangeRoom roomChangerDToU = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerDToU.limitChangeBottom = 22;
            roomChangerDToU.limitChangeUpper = 22;
            roomChangerDToU.limitChangeLeft = 0;
            roomChangerDToU.limitChangeRight = 0;
        }
        else if (count == 3)
        {
            ChangeRoom roomChangerLToU = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerLToU.limitChangeBottom = 22;
            roomChangerLToU.limitChangeUpper = 22;
            roomChangerLToU.limitChangeLeft = 0;
            roomChangerLToU.limitChangeRight = 0;

            ChangeRoom roomChangerUToR = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerUToR.limitChangeBottom = 0;
            roomChangerUToR.limitChangeUpper = 0;
            roomChangerUToR.limitChangeLeft = 22;
            roomChangerUToR.limitChangeRight = 22;

            ChangeRoom roomChangerRToD = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerRToD.limitChangeBottom = -22;
            roomChangerRToD.limitChangeUpper = -22;
            roomChangerRToD.limitChangeLeft = 0;
            roomChangerRToD.limitChangeRight = 0;

            ChangeRoom roomChangerDToL = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerDToL.limitChangeBottom = 0;
            roomChangerDToL.limitChangeUpper = 0;
            roomChangerDToL.limitChangeLeft = -22;
            roomChangerDToL.limitChangeRight = -22;
        }
    }
}
