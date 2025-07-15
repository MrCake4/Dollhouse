using UnityEngine;
using System.Collections;

public class SquishEffect : MonoBehaviour
{
    [SerializeField] private float squishAmount = 0.3f;
    [SerializeField] private float squishDuration = 0.2f;

    private Vector3 originalScale;
    private Coroutine squishRoutine;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void SquishOnce()
    {
        Debug.Log("Squish effect triggered on " + gameObject.name);
        if (squishRoutine != null)
            StopCoroutine(squishRoutine);

        squishRoutine = StartCoroutine(SquishCoroutine());
    }

    private IEnumerator SquishCoroutine()
    {
        float timer = 0f;

        Vector3 squishedScale = new Vector3(
            originalScale.x + squishAmount,
            originalScale.y - squishAmount,
            originalScale.z + squishAmount
        );

        // Squish
        while (timer < squishDuration / 2f)
        {
            transform.localScale = Vector3.Lerp(originalScale, squishedScale, timer / (squishDuration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = squishedScale;
        timer = 0f;

        // Unsquish
        while (timer < squishDuration / 2f)
        {
            transform.localScale = Vector3.Lerp(squishedScale, originalScale, timer / (squishDuration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        squishRoutine = null;
    }
}
