using DiasGames.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damaged : MonoBehaviour
{
    [SerializeField] int damagePoints;
    [SerializeField] bool onAttack = false;
    [SerializeField] Collider HitBox;
    [SerializeField] AudioSource audioclip;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<IDamage>() != null)
        {
            
            if (!onAttack) 
            {
                other.gameObject.GetComponent<IDamage>().Damage(damagePoints);
                onAttack = true;
                Debug.Log("Atack");
                HitBox.enabled = false;
                audioclip.Play();
            }
            
        }
    }
    public void setUp()
    {
        onAttack = false;
        HitBox.enabled = true;
    }
}
