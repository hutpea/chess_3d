using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public bool isEnableSelect;

    private void Awake()
    {
        isEnableSelect = true;
    }

    void Update()
    {
        if (!isEnableSelect)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.collider != null)
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    if (hitObject.tag == "Piece")
                    {
                        if (hitObject.GetComponent<Piece>() != null)
                        {
                            GameplayController.Instance.board.DeselectAllPieces();
                            hitObject.GetComponent<Piece>().ToggleOutline(true);
                        }
                    }
                    else if(hitObject.tag == "Cell")
                    {
                        Debug.Log("Click on a cell !");
                        Vector2Int selectedPosition = hitObject.GetComponent<Cell>().position; //lay toa do o dc chon
                        GameplayController.Instance.board.MoveCurrentSelectedPieceTo(selectedPosition);
                    }
                }
            }
            else
            {
                Debug.Log("not hit");
            }
        }
    }
}
