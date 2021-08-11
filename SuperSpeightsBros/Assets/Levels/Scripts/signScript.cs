using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signScript : MonoBehaviour
{


    public GameObject Popup;
    public Text dialogue;

    public string dialog;

    public bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && playerInRange)
        {
            if(Popup.activeInHierarchy){

                Popup.SetActive(false);

            }else{
                Popup.SetActive(true);
                dialogue.text = dialog;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerInRange = false;
            Popup.SetActive(false);
            


        }
    }
}
