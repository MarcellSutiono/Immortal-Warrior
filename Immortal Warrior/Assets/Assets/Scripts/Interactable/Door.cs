using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public bool isFirstDoor = false;
    public Transform target;
    public GameObject player;
    public GameObject oldBossUI;
    public GameObject newBossUI;
    public Image fadeTransition;
    private bool playerInside = false;

    public GameObject audioObject;
    private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void Start()
    {
        audioManager = audioObject.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(TeleportWithFade());
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        Color c = fadeTransition.color;
        float t = 0;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            fadeTransition.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        Color c = fadeTransition.color;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / duration);
            fadeTransition.color = c;
            yield return null;
        }
    }


    private IEnumerator TeleportWithFade()
    {
        yield return StartCoroutine(FadeIn(2f));

        player.transform.position = target.position;
        yield return new WaitForSeconds(0.5f);

        if(isFirstDoor)
            audioManager.changeMusic(audioManager.gameMusic);
            
        yield return StartCoroutine(FadeOut(2f));

        if(oldBossUI)
            oldBossUI.SetActive(false);

        if(newBossUI)
            newBossUI.SetActive(true);
    }
}
