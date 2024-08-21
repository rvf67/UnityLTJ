using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissilePool : ObjectPool<BossMissile>
{
    // BossMissile는 enemy지만 점수를 받지 않을 것이므로 EnemyObjectPool을 쓰지않음
    //굳이 추가할 필요가 없음
}
