using UnityEngine;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Grid Coordinates")]
    [SerializeField] private int x;
    [SerializeField] private int y;

    [Header("Label Settings")]
    private TextMeshPro label;

    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    void Start()
    {
        // Create the label dynamically
        if (label == null)
        {
            GameObject newLabel = new GameObject("GridLabel");
            newLabel.transform.SetParent(transform);

            // Set Y position to 0.5 and flat
            newLabel.transform.localPosition = new Vector3(0, .75f, 0);
            newLabel.transform.localEulerAngles = new Vector3(90, 0, 0);

            label = newLabel.AddComponent<TextMeshPro>();
            label.fontSize = 2;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.white;
        }

        if (label != null)
        {
            label.text = $"x: {x}, y: {y}";
        }
    }
}

