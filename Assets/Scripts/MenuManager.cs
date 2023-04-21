using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public AudioSource start;
    public AudioSource quit;
    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetInstance().lastChek && !once)
        {
            StartCoroutine(StartGame());
            anim.SetTrigger("Fade");
            start.Play();
            once = true;
        }
        if (InputManager.GetInstance().Reset && !once)
        {
            StartCoroutine(QuitGame());
            quit.Play();
            once = true;
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
