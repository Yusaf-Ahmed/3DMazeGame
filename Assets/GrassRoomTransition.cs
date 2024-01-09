using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GrassRoomTransition : MonoBehaviour
{
    [SerializeField] private string GrassRoom;
    public Animator anim;
    public float loading = 1.5f;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           StartCoroutine(LoadGame());
        }
    }

    private IEnumerator LoadGame()
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(loading);  
        SceneManager.LoadScene(GrassRoom);
    }
   
}
