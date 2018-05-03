using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListListen : MonoBehaviour
{

    List<Room> roomList = new List<Room>();

    public GameObject room;
    public GameObject content;

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            AddRoom("房间" + i, "房主" + i, 4, 1);
        }

        RemoveRoom(roomList[2]);
    }

    //添加列表项  
    public void AddRoom(string roomName, string hostName, int playerNum, int currentNum)
    {
        GameObject a = Instantiate(room) as GameObject;
        a.transform.Find("Button").transform.Find("RoomName").GetComponent<Text>().text = roomName;
        a.transform.Find("Button").transform.Find("HostName").GetComponent<Text>().text = hostName;
        a.transform.Find("Button").transform.Find("PlayerNum").GetComponent<Text>().text = currentNum + "/" + playerNum;

        a.GetComponent<Transform>().SetParent(content.GetComponent<Transform>(), false);

        a.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
            delegate () {
                print("你选中了房间" + hostName);
            }
        );

        Room r = new Room(a, roomName, hostName, playerNum, currentNum);
        roomList.Add(r);
    }

    //移除列表项  
    public void RemoveRoom(Room t)
    {
        int index = roomList.IndexOf(t);
        roomList.Remove(t);
        Destroy(t.room);
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {

        }
    }
}

public class Room
{
    public GameObject room;
    string roomName;
    string hostName;
    int playerNum;
    int currentNum;

    public Room(GameObject room, string roomName, string hostName, int playerNum, int currentNum)
    {
        this.room = room;
        this.roomName = roomName;
        this.hostName = hostName;
        this.playerNum = playerNum;
        this.currentNum = currentNum;
    }
};
