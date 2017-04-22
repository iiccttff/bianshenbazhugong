/*
 * 脚本名(ScriptName)：    CharacterTestScript2.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class CharacterTestScript2 : MonoBehaviour 
{
    Animator ani;
	// Use this for initialization
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetBool("run", true);
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            ani.SetBool("run", false);
        }
    }
}
