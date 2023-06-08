using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CanEditMultipleObjects()]
[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererSortingLayersEditor : Editor
{
    private Renderer _renderer;
    private string[] _sortingLayerNames;
    private int _selectedOption;

    void OnEnable()
    {
        _sortingLayerNames = GetSortingLayerNames();
        _renderer = (target as Renderer).gameObject.GetComponent<Renderer>();
        //light2d = (target as DynamicLight);

        for (int i = 0; i < _sortingLayerNames.Length; i++)
        {
            if (_sortingLayerNames[i] == _renderer.sortingLayerName)
                _selectedOption = i;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!_renderer) return;

        EditorGUILayout.BeginHorizontal();
        _selectedOption = EditorGUILayout.Popup("Sorting Layer", _selectedOption, _sortingLayerNames);
        if (_sortingLayerNames[_selectedOption] != _renderer.sortingLayerName)
        {
            Undo.RecordObject(_renderer, "Sorting Layer");
            _renderer.sortingLayerName = _sortingLayerNames[_selectedOption];
            EditorUtility.SetDirty(_renderer);
        }
        EditorGUILayout.LabelField("(Id:" + _renderer.sortingLayerID.ToString() + ")", GUILayout.MaxWidth(40));
        EditorGUILayout.EndHorizontal();

        int newSortingLayerOrder = EditorGUILayout.IntField("Order in Layer", _renderer.sortingOrder);
        if (newSortingLayerOrder != _renderer.sortingOrder)
        {
            Undo.RecordObject(_renderer, "Edit Sorting Order");
            _renderer.sortingOrder = newSortingLayerOrder;
            EditorUtility.SetDirty(_renderer);
        }
    }

    // Get the sorting layer names
    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

}