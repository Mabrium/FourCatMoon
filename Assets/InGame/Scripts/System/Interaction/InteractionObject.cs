using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private List<Transform> transforms = new List<Transform>();

     void Interactioin()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "Player")
        {
            if (!transforms.Contains(collision.transform))
            {
                transforms.Add(collision.transform);
            }
        }
    }
}
