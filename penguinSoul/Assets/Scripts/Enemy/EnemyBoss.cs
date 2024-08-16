using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : RecycleObject
{
    private int patternRandom = 0;
    int totalPatternCount;
    private static readonly int NONE = 0; 
    private static readonly int PATTERN1 = 1; 
    private static readonly int PATTERN2 = 2; 
    private static readonly int PATTERN3 = 3; 
    private static readonly int PATTERN4 = 4;

    IEnumerator RandomPattern()
    {
        switch (patternRandom)
        {
            case 1:
                StartCoroutine(Pattern1());
                break;
            case 2:
                StartCoroutine(Pattern2());
                break;
            case 3:
                StartCoroutine(Pattern3());
                break;
            case 4:
                StartCoroutine(Pattern4());
                break;
        }
        yield return null;
    }
    IEnumerator Pattern1() { yield return null; }
    IEnumerator Pattern2() { yield return null; }
    IEnumerator Pattern3() { yield return null; }
    IEnumerator Pattern4() { yield return null; }
}
