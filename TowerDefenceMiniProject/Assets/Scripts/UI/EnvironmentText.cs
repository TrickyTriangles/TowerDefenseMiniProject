using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentText : Singleton<EnvironmentText>
{
    public enum TextTypes
    {
        NORMAL = 0,
        SLOW,
        FREEZE,
        BURN,
        EXPERIENCE
    }

    public enum TextModifiers
    {
        NONE = 0,
        CRITICAL = 1
    }

    [SerializeField] private TextMeshProUGUI[] text_prefabs;
    private Dictionary<int, TextMeshProUGUI> text_prefab_dictionary;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        text_prefab_dictionary = new Dictionary<int, TextMeshProUGUI>();

        for (int i = 0; i < text_prefabs.Length; i++)
        {
            text_prefab_dictionary.Add(i, text_prefabs[i]);
        }
    }

    public void SetCanvas(Canvas new_canvas)
    {
        canvas = new_canvas;
    }

    public void SetCamera(Camera new_camera)
    {
        _camera = new_camera;
    }

    public void DrawText(string message, TextTypes type, Vector3 position)
    {
        TextMeshProUGUI t_prefab = text_prefab_dictionary[(int)type];

        if (t_prefab != null)
        {
            TextMeshProUGUI new_text = Instantiate(t_prefab, position, Quaternion.identity, canvas.transform);

            new_text.rectTransform.position = _camera.WorldToScreenPoint(position);
            new_text.text = message;
        }
    }

    public void DrawText(string message, TextTypes type, Vector3 position, TextModifiers modifier)
    {
        TextMeshProUGUI t_prefab = text_prefab_dictionary[(int)type];

        if (t_prefab != null)
        {
            TextMeshProUGUI new_text = Instantiate(t_prefab, position, Quaternion.identity, canvas.transform);

            new_text.rectTransform.position = _camera.WorldToScreenPoint(position);
            new_text.text = message;

            switch (modifier)
            {
                case TextModifiers.NONE:
                    break;
                case TextModifiers.CRITICAL:
                    new_text.fontSize *= 1.5f;
                    break;
                default:
                    break;
            }
        }
    }
}
