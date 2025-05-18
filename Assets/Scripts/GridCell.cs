using UnityEngine;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Grid Coordinates")]
    [SerializeField] private int x;
    [SerializeField] private int y;

    [Header("Grid Data")]
    [SerializeField] private int idOfOwner;  
    [SerializeField] private string ownerNickname;

    [Header("Label Settings")]
    private TextMeshPro label;
    private Renderer renderer;

    public int X => x;
    public int Y => y;

    /// <summary>
    /// Initializes the basic grid coordinates
    /// </summary>
    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;

        // Get the renderer for color manipulation
        renderer = GetComponent<Renderer>();
    }


        /// <summary>
    /// Called to set additional data received from the server
    /// </summary>
    /// <param name="data">GridData object containing id, x, y, and owner</param>
    public void SetData(GridData data)
    {
        idOfOwner = data.ownerOfTheGridId;  
        ownerNickname = data.ownerOfTheGridNickname;

        // Create the label if it doesn't exist
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

        // Update the label text
        label.text = $"ID: {idOfOwner}\nOwner: {ownerNickname}\n(x: {x}, y: {y})";

            Debug.Log("Sesionid " + SessionData.UserId + " id " + idOfOwner);
            // ✅ Change color based on ownership
            if (SessionData.UserId == idOfOwner)
            {
                SetColor(Color.green); // Owned by current user
            }
            else
            {
                SetColor(new Color(1f, 0.65f, 0f)); // Orange color
            }
    }


    /// <summary>
    /// Sets the color of the grid cell
    /// </summary>
    /// <param name="color">Color to set</param>
    private void SetColor(Color color)
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
