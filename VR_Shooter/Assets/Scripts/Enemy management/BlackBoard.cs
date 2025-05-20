using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
   public void Broadcast(Transform target)
    {
        BroadcastMessage("FollowTarget", target);

    }
}
