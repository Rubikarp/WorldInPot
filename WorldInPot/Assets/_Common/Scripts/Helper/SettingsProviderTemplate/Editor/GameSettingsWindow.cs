#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSettingsWindow : EditorWindow
{
    [SerializeField] private int selectedIndex = -1;
    private ListView leftPanel;
    private VisualElement rightView;

    [MenuItem("WorldGame/GameSettingsWindow")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor.
        EditorWindow wnd = GetWindow<GameSettingsWindow>();
        wnd.titleContent = new GUIContent("GameSettingsWindow");

        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        //wnd.maxSize = new Vector2(1920, 1080);
    }

    public void CreateGUI()
    {
        // Get a list of all GameSettings in the project.
        Type[] types = ReflectionHelper.GetTypesWithAttribute<GameSettingsAttribute>();
        List<ScriptableObject> list = new List<ScriptableObject>();
        foreach (var type in types)
        {
            list.Add(type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null, null) as ScriptableObject);
        }

        // Create a two-pane view with the left pane being fixed.
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the panel to the visual tree by adding it as a child to the root element.
        rootVisualElement.Add(splitView);

        // A TwoPaneSplitView always needs two child elements.
        leftPanel = new ListView();
        splitView.Add(leftPanel);
        rightView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        splitView.Add(rightView);

        // Initialize the list view with all sprites' names.
        leftPanel.makeItem = () => new Label();
        leftPanel.bindItem = (item, index) => { (item as Label).text = list[index].name; };
        leftPanel.itemsSource = list;

        // React to the user's selection.
        leftPanel.selectionChanged += OnGameSettingSelectionChange;
        // Store the selection index when the selection changes.
        leftPanel.selectionChanged += (items) => { selectedIndex = leftPanel.selectedIndex; };

        // Restore the selection index from before the hot reload.
        leftPanel.selectedIndex = selectedIndex;
    }

    private void OnValidate()
    {
        leftPanel.selectedIndex = selectedIndex;
    }
    private void OnGameSettingSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane.
        rightView.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var selectedScriptable = enumerator.Current as ScriptableObject;
            if (selectedScriptable != null)
            {
                // Draw the instance using Unity UI Toolkit
                var visualElement = new VisualElement();

                // Add the object field of the scriptable.
                var objectField = new UnityEditor.UIElements.ObjectField
                {
                    objectType = selectedScriptable.GetType(),
                    value = selectedScriptable
                };
                visualElement.Add(objectField);

                // Add the content of the scriptable.
                var imguiContainer = new IMGUIContainer(() =>
                {
                    Editor editor = Editor.CreateEditor(selectedScriptable);
                    editor.OnInspectorGUI();
                });
                visualElement.Add(imguiContainer); rightView.Add(visualElement);
                rightView.Add(visualElement);
            }
        }
    }
}
#endif