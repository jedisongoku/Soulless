using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public delegate void RespawnAction();
    public static event RespawnAction OnRespawn;

}
