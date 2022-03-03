using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    //1
    public GameBehavior gameManager;

    void Start()
    {
        //2
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    // Start is called before the first frame update
    {
        //2
        if(collision.gameObject.name == "Player")

    // Update is called once per frame
        {
            //3
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Item collected!");

            //3
            gameManager.Items += 1;
            gameManager.PrintLootReport();
        }
    }
}
