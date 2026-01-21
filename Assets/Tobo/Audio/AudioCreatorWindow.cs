#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static System.IO.Path;
using System.IO;
using System.Linq;

namespace Tobo.Audio.Editor
{
    public class AudioCreatorWindow : EditorWindow
    {
        static readonly string BasePath = "Assets/Sounds/";
        string pathExtension = string.Empty;

        [SerializeField]
        List<AudioClip> clips = new List<AudioClip>();
        string fileName;
        AudioCategory category = AudioCategory.SFX;

        bool batchMode = true;
        bool doCodegen = true;

        SerializedObject target;

        [MenuItem("Audio/Import new AudioClips")]
        [MenuItem("Assets/Create/Audio/Import new AudioClips")]
        public static void ShowWindow()
        {
            AudioCreatorWindow window = EditorWindow.GetWindow<AudioCreatorWindow>();
            window.titleContent = new GUIContent("Import Audio");
            window.minSize = new Vector2(300, 450);

            window.clips = Selection.GetFiltered<AudioClip>(SelectionMode.Assets).ToList();
        }

        private void OnEnable()
        {
            target = new SerializedObject(this);
        }

        private void OnGUI()
        {
            // Edit clips list
            target.Update();
            SerializedProperty prop = target.FindProperty(nameof(clips));
            EditorGUILayout.PropertyField(prop, true);
            target.ApplyModifiedProperties();

            if (clips.Count == 0 || clips.All((c) => c == null))
            {
                EditorGUILayout.LabelField("Please choose at least 1 audio clip.");
                return;
            }

            string catBasePath = Combine(BasePath, category.ToString());
            //DisabledLabel("Base path:", catBasePath);
            // If users want to put this into a sub-folder
            EditorGUILayout.LabelField("Folder", EditorStyles.boldLabel);
            pathExtension = EditorGUILayout.TextField("Subfolders", pathExtension);
            DisabledLabel("Folder path:", catBasePath + "/" + pathExtension ?? string.Empty);

            // Make them choose a name and batch mode setting
            if (clips.Count > 1)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Multiple Files", EditorStyles.boldLabel);
                batchMode = EditorGUILayout.Toggle("Save each file separately", batchMode);
                if (!batchMode)
                {
                    EditorGUILayout.LabelField("All clips will be variations of one Sound.");
                    fileName = EditorGUILayout.TextField("Sound Name", fileName);
                }
            }

            if (clips.Count > 0 && clips[0] != null)
                DisplaySaveGUI();
        }

        void DisplaySaveGUI()
        {
            // They need a filename if there are multiple sounds in one file
            if (clips.Count > 1 && !batchMode && string.IsNullOrEmpty(fileName))
            {
                EditorGUILayout.LabelField("Please choose a valid name");
                return;
            }

            string catBasePath = Combine(BasePath, category.ToString());
            string path = Combine(catBasePath, pathExtension ?? string.Empty);
            if (clips.Count == 1)
                fileName = clips[0].name;
            if (clips.Count == 1 || !batchMode)
                DisabledLabel("Asset Path", path + "/" + fileName);
            else
                DisabledLabel("Asset Paths", path + "/(multiple)");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sound Data", EditorStyles.boldLabel);
            category = (AudioCategory)EditorGUILayout.EnumPopup("Audio Category", category);
            EditorGUILayout.Space();
            doCodegen = EditorGUILayout.Toggle(new GUIContent("Generate Sound.ID code",
                "Auto regenerates the file containing sound IDs. If disabled, run Audio/Update Current Sounds"), doCodegen);

            if (clips.Count == 1 || !batchMode)
            {
                if (GUILayout.Button("Save sound"))
                    Save(path, fileName);
            }
            else if (GUILayout.Button($"Save {clips.Count} sounds"))
                SaveBatch(path);
        }

        void Save(string path, string fileName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Sound s = Sound.CreateInternal(fileName, clips, category);
            fileName += ".asset";
            AssetDatabase.CreateAsset(s, Combine(path, fileName));
            AssetDatabase.Refresh();
            SoundLibrary.FillAndGenerateSounds(doCodegen);
        }

        void SaveBatch(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < clips.Count; i++)
            {
                List<AudioClip> _new = new List<AudioClip>();
                _new.Add(clips[i]);
                Sound s = Sound.CreateInternal(clips[i].name, _new, category);
                AssetDatabase.CreateAsset(s, Combine(path, clips[i].name + ".asset"));
            }
            AssetDatabase.Refresh();
            SoundLibrary.FillAndGenerateSounds(doCodegen);
        }

        void DisabledLabel(string label, string text)
        {
            GUI.enabled = false;
            EditorGUILayout.TextField(label, text);
            GUI.enabled = true;
        }
    }
}
#endif
