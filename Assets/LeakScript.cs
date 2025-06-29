using System.Collections;
using UnityEngine;

public class LeakScript : MonoBehaviour
{
    public GameManagerScript gameManager;

    bool isLoosingWater = false;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManagerScript>();
        gameManager.UpdateDamLeakStatus(1);
    }

    void FixedUpdate()
    {
        if (!isLoosingWater)
        {

        }
    }

    IEnumerator LooseWater() {
    yield return new WaitForSeconds(2);
        gameManager.UpdateWaterLevel(3);
    Debug.Log("FINISHED");
    }
}
