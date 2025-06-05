using UnityEngine;
using UnityEngine.UI;

public class RobotPartController : MonoBehaviour
{
    [Header("Fixed Points on Body")]
    public Transform[] fixedPoints;

    [Header("Detached Parts (each must have 1 child)")]
    public Transform[] detachedParts;

    [Header("Explode Destination")]
    public Transform explodePlane;

    [Header("UI Elements")]
    public GameObject popupPanel;
    public Button closeButton;

    [Header("Snap Settings")]
    public float snapDistance = 0.15f;

    [Header("Animation Settings")]
    public Animator robotAnimator;
    public string animationName = "wave"; // Name of your animation

    private Transform[] detachedAttachPoints;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private bool[] partLocked;

    private Camera arCamera;
    private Transform draggedPart;
    private bool exploded = false;

    void Start()
    {
        arCamera = Camera.main;

        int partCount = detachedParts.Length;
        detachedAttachPoints = new Transform[partCount];
        originalPositions = new Vector3[partCount];
        originalRotations = new Quaternion[partCount];
        partLocked = new bool[partCount];

        for (int i = 0; i < partCount; i++)
        {
            if (detachedParts[i].childCount == 1)
            {
                detachedAttachPoints[i] = detachedParts[i].GetChild(0);
            }
            else
            {
                Debug.LogError($"{detachedParts[i].name} must have exactly 1 child.");
            }

            originalPositions[i] = detachedParts[i].position;
            originalRotations[i] = detachedParts[i].rotation;
            partLocked[i] = true;
        }

        popupPanel.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnPopupCloseClicked);
        }
    }

    void Update()
    {
        if (!exploded) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began && Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < detachedParts.Length; i++)
                {
                    if (hit.transform == detachedParts[i] && !partLocked[i])
                    {
                        draggedPart = detachedParts[i];
                        break;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && draggedPart != null)
            {
                Plane plane = new Plane(arCamera.transform.forward * -1, draggedPart.position);
                if (plane.Raycast(ray, out float enter))
                {
                    draggedPart.position = ray.GetPoint(enter);
                }
            }
            else if (touch.phase == TouchPhase.Ended && draggedPart != null)
            {
                int partIndex = System.Array.IndexOf(detachedParts, draggedPart);

                if (partIndex != -1)
                {
                    float dist = Vector3.Distance(detachedAttachPoints[partIndex].position, fixedPoints[partIndex].position);

                    if (dist < snapDistance && !partLocked[partIndex])
                    {
                        SnapPartToFixedPoint(partIndex);
                        partLocked[partIndex] = true;

                        if (AllPartsLocked())
                            popupPanel.SetActive(true);
                    }
                }

                draggedPart = null;
            }
        }
    }

    void SnapPartToFixedPoint(int i)
    {
        Vector3 offset = detachedAttachPoints[i].position - detachedParts[i].position;
        detachedParts[i].position = fixedPoints[i].position - offset;
        detachedParts[i].rotation = fixedPoints[i].rotation;
    }

    public void ExplodeParts()
    {
        if (explodePlane == null)
        {
            Debug.LogWarning("Explode plane not assigned!");
            return;
        }

        exploded = true;
        float spacing = 0.5f; // Increased spacing for better separation
        Vector3 basePos = explodePlane.position;

        for (int i = 0; i < detachedParts.Length; i++)
        {
            detachedParts[i].position = basePos + new Vector3((i - detachedParts.Length / 2f) * spacing, 0.1f, 0);
            detachedParts[i].rotation = Quaternion.identity;
            partLocked[i] = false;
        }

        popupPanel.SetActive(false);
    }

    public void ResetParts()
    {
        exploded = false;

        for (int i = 0; i < detachedParts.Length; i++)
        {
            detachedParts[i].position = originalPositions[i];
            detachedParts[i].rotation = originalRotations[i];
            partLocked[i] = true;
        }

        popupPanel.SetActive(false);
    }

    bool AllPartsLocked()
    {
        foreach (bool locked in partLocked)
        {
            if (!locked) return false;
        }
        return true;
    }

    private void OnPopupCloseClicked()
    {
        popupPanel.SetActive(false);

        if (robotAnimator != null)
        {
            robotAnimator.SetTrigger("PlayWave");
        }
    }
}
