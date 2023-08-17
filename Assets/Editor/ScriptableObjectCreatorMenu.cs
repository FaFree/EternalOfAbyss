﻿namespace Multicast.EditorTools {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    public class ScriptableObjectCreatorMenu : OdinMenuEditorWindow {
        private static readonly HashSet<Type> ScriptableObjectTypes = new HashSet<Type>(AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(ScriptableObject).IsAssignableFrom(t) &&
                !typeof(EditorWindow).IsAssignableFrom(t) &&
                !typeof(Editor).IsAssignableFrom(t)));

        [MenuItem("Assets/Create Scriptable Object...", priority = -10000)]
        private static void ShowDialog() {
            var path = "Assets";
            var obj  = Selection.activeObject;
            if (obj && AssetDatabase.Contains(obj)) {
                path = AssetDatabase.GetAssetPath(obj);
                if (!Directory.Exists(path)) {
                    path = Path.GetDirectoryName(path);
                }
            }

            var window = CreateInstance<ScriptableObjectCreatorMenu>();
            window.ShowUtility();
            window.position     = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.titleContent = new GUIContent(path);
            window.targetFolder = path?.Trim('/');
        }

        private ScriptableObject previewObject;
        private string           targetFolder;
        private Vector2          scroll;

        private Type SelectedType {
            get {
                var m = this.MenuTree.Selection.LastOrDefault();
                return m == null ? null : m.Value as Type;
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            this.MenuWidth     = 270;
            this.WindowPadding = Vector4.zero;

            OdinMenuTree tree = new OdinMenuTree(false);
            tree.Config.DrawSearchToolbar = true;
            tree.DefaultMenuStyle         = OdinMenuStyle.TreeViewStyle;
            tree.AddRange(ScriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType);
            tree.SortMenuItemsByName();
            tree.Selection.SelectionConfirmed += x => this.CreateAsset();
            tree.Selection.SelectionChanged += e => {
                if (this.previewObject && !AssetDatabase.Contains(this.previewObject)) {
                    DestroyImmediate(this.previewObject);
                }

                if (e != SelectionChangedType.ItemAdded) {
                    return;
                }

                var t = this.SelectedType;
                if (t != null && !t.IsAbstract) {
                    this.previewObject = CreateInstance(t);
                }
            };

            return tree;
        }

        private string GetMenuPathForType(Type t) {
            var path = "";

            if (t.Namespace != null) {
                foreach (var part in t.Namespace.Split('.')) {
                    path += part.SplitPascalCase() + "/";
                }
            }

            return path + t.Name.SplitPascalCase();
        }

        protected override IEnumerable<object> GetTargets() {
            yield return this.previewObject;
        }

        protected override void DrawEditor(int index) {
            this.scroll = GUILayout.BeginScrollView(this.scroll);
            {
                base.DrawEditor(index);
            }
            GUILayout.EndScrollView();

            if (this.previewObject) {
                GUILayout.FlexibleSpace();
                SirenixEditorGUI.HorizontalLineSeparator();
                if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30))) {
                    this.CreateAsset();
                }
            }
        }

        private void CreateAsset() {
            if (this.previewObject) {
                var dest = this.targetFolder + "/" + this.MenuTree.Selection.First().Name + ".asset";
                dest = AssetDatabase.GenerateUniqueAssetPath(dest);
                ProjectWindowUtil.CreateAsset(this.previewObject, dest);
                EditorApplication.delayCall += this.Close;
            }
        }
    }
}