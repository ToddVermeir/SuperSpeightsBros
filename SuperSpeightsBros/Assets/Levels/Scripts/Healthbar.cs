using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Slider health_slide;

    public void SetMaxHealth(int health){
        health_slide.maxValue = health;
        health_slide.value = health;
    }

    public void SetHealth(int health){
        health_slide.value = health; 
    }

}
