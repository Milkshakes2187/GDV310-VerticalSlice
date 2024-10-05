using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    public float HP = 100.0f;

    public TextMeshProUGUI textMeshPro;
    public void TakeDamage(float _damage)
    {
        HP -= _damage;

        //update the text
        textMeshPro.text = HP.ToString();
        if(HP <= 0.0f)
        {
            Destroy(this);
        }
    }

   
}
