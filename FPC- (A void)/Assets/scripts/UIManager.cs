using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Camera mainCamera;
    public float rotationSpeed = 3f;
    private bool isRotating = false;
    public GameObject[] UIElements;
    public GameObject initialButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RotateCameraLeftBy90Degrees()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateCameraCouroutine(90f));
        }
    }
    private IEnumerator RotateCameraCouroutine(float angle)
    {
        isRotating = true;

        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -angle, 0);

        float rotationProgress = 0f;
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / angle); //normalizes the rotation speed based on angle
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress); //smooth interpolation of rotation
            yield return null;
        }
        mainCamera.transform.rotation = endRotation; //ensure final rotation
        isRotating = false;

        initialButton.SetActive(false);

        foreach (GameObject UIElement in UIElements)
        {
            UIElement.SetActive(true);
        }
    }
}
