using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHorde : MonoBehaviour
{
    public int oilNumber;

    public GameObject hordeOne, hordeTwo, hordeThree;
    private PlayerStats player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.oilCount > 0)
            {
                switch (oilNumber)
                {
                    case 1:
                           hordeOne.SetActive(true);
                        break;
                    case 2:
                        hordeTwo.SetActive(true);
                        break;
                    case 3:
                        hordeThree.SetActive(true);
                        break;
                }
            }
        }
    }
}
