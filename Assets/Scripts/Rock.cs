using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;     //rock's hp

    [SerializeField]
    private float destroyTime;  //amount of time for crashing

    [SerializeField]
    private SphereCollider col;     //Sphere collider for rock

    [SerializeField]
    private GameObject go_rock;     //gameobject_rock

    [SerializeField]
    private GameObject go_debris;   //gameobject_debris

    [SerializeField]
    private GameObject go_effect;   //Mining effect

    [SerializeField]
    private GameObject go_rock_item_prefab;     //obtainable rock item


    //Sound effects
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        var clone = Instantiate(go_effect, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        col.enabled = false;
        for (int i = 0; i < Mathf.Round(Random.Range(0, 5)); i++)
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
