using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_prefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera _camera;

    public void CreateNewEnvironmentText(string message)
    {
        TextMeshProUGUI new_text = Instantiate(text_prefab, transform.position, Quaternion.identity, canvas.transform);

        new_text.rectTransform.position = _camera.WorldToScreenPoint(transform.position);
        new_text.text = message;
    }
}
