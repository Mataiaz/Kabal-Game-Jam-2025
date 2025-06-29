using UnityEngine;

public class DamWallScript : MonoBehaviour
{
    public int health = 250;
    int countUntilHole = 3;
    public GameManagerScript gameManager;
    public Transform wallCrackPlacement;
    public GameObject wallCrack;

  public void LooseHealth(int substraction)
    {

        if (health - substraction <= 0)
        {
            gameManager.GameOver();
        }
        else
        {
            health = health - substraction;
            gameManager.UpdateDamHealth();
            countUntilHole--;
        }

        if (countUntilHole <= 0)
        {
            countUntilHole = 3;
            Instantiate(
                wallCrack,
                new Vector3(
                    wallCrackPlacement.position.x,
                    wallCrackPlacement.position.y + Random.Range(-1f, 1f),
                    wallCrackPlacement.position.z + Random.Range(-1f, 1f)
                ),
                Quaternion.Euler(
                    wallCrackPlacement.rotation.eulerAngles + new Vector3(
                        Random.Range(-50f, 50f),
                        wallCrackPlacement.rotation.y,
                        wallCrackPlacement.rotation.z
                    )
                )
            );
        }
    }
}
