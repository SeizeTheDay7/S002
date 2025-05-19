using UnityEngine;
using System.Collections;

public class GameEndSequence : MonoBehaviour
{
    [SerializeField] SheepMoveGameEnd sheepMoveGameEnd;
    [SerializeField] GameObject exitFence;
    [SerializeField] GameObject congrats;
    [SerializeField] GameObject firecracker1;
    [SerializeField] GameObject firecracker2;


    public void PlayGameEndSequence()
    {
        StartCoroutine(GameEndCoroutine());
    }

    private IEnumerator GameEndCoroutine()
    {
        yield return new WaitForSeconds(3f);
        firecracker1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        firecracker2.SetActive(true);
        yield return new WaitForSeconds(2f);
        congrats.SetActive(true);
        firecracker1.GetComponent<ParticleSystem>().Play();
        firecracker2.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(7f);
        firecracker2.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        firecracker1.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        congrats.SetActive(false);
        yield return new WaitForSeconds(2f);
        exitFence.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        sheepMoveGameEnd.enabled = true;
    }
}