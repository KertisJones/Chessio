using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public int sizeX = 8;
    public int sizeY = 8;

    public float offsetX = 0;
    public float offsetY = 0;

    public GameObject blackSquare;
    public GameObject whiteSquare;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0))
                {
                    if (blackSquare != null)
                        Instantiate(blackSquare, new Vector3(x + offsetX, y + offsetY, 0), Quaternion.identity);
                }
                else
                {
                    if (whiteSquare != null)
                        Instantiate(whiteSquare, new Vector3(x + offsetX, y + offsetY, 0), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
