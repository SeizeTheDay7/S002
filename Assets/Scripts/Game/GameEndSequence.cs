using UnityEngine;
using System.Collections;

public class GameEndSequence : MonoBehaviour
{
    [SerializeField] SheepMoveGameEnd sheepMoveGameEnd;
    [SerializeField] GameObject exitFence;
    [SerializeField] GameObject exitTrigger;

    public void PlayGameEndSequence()
    {
        StartCoroutine(GameEndCoroutine());
    }

    private IEnumerator GameEndCoroutine()
    {
        yield return new WaitForSeconds(2f);
        exitFence.SetActive(false);
        exitTrigger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sheepMoveGameEnd.enabled = true;
    }
}