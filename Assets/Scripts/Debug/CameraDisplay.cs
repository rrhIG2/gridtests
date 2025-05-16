using UnityEngine;
using TMPro;

public class CameraDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI cameraPositionText;
    public Transform cameraTransform;

    void Update()
    {
        if (cameraTransform != null && cameraPositionText != null)
        {
            cameraPositionText.text = $"Pos: {cameraTransform.position}Rot: {cameraTransform.rotation.eulerAngles}";
        }
    }
}

