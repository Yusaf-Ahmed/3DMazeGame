using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    public static int Camera;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(StartLetAnimationPlay());
        Camera = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera == 1)
        {
            GetComponent<Animator>().enabled = true;
            anim.SetInteger("CameraAngle", 1);
            anim.SetInteger("CameraAngle", 0);
            GetComponent<Animator>().enabled = false;
            Camera = 0;
        }

        if (Camera == 2)
        {
            StartCoroutine(LetAnimationPlay());
            anim.SetInteger("CameraAngle", 0);
            GetComponent<Animator>().enabled = false;
            Camera = 0;
        }
    }

    private IEnumerator LetAnimationPlay()
    {
        GetComponent<Animator>().enabled = true;
        anim.SetInteger("CameraAngle", 2);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator StartLetAnimationPlay()
    {
        Camera = 1;
        yield return new WaitForSeconds(1);
    }
}
