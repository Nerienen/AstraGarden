using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectUtils.Helpers;
using UnityEngine;

public class UnityDelay : MonoBehaviour
{
    public static Task Delay(int millisecondsDelay)
    {
        if (millisecondsDelay < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay), "Delay cannot be negative.");
        }

        var tcs = new TaskCompletionSource<bool>();

        var coroutine = DelayCoroutine(millisecondsDelay, () => tcs.SetResult(true));
        CoroutineController.Start(coroutine);

        return tcs.Task;
    }

    private static IEnumerator DelayCoroutine(int millisecondsDelay, Action onCompleted)
    {
        var targetTime = Time.realtimeSinceStartup + (millisecondsDelay / 1000f);

        while (Time.realtimeSinceStartup < targetTime)
        {
            yield return null;
        }

        onCompleted?.Invoke();
    }
}
