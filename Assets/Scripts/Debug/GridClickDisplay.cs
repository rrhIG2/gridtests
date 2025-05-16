using UnityEngine;
using TMPro;

public class GridClickDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI gridPositionText;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Ray ray;

            if (Input.touchCount > 0)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridCell gridCell = hit.collider.GetComponent<GridCell>();
                if (gridCell != null)
                {
                    gridPositionText.text = $"Grid Position: X = {gridCell.X}, Y = {gridCell.Y}";
                }
            }
        }
    }
}

