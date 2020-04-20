
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityTools
{
    public class SceneSelectDropdown : EditorWindow
    {
        private bool m_showOnlyScenesInBuild = false;

        //---------------------------------------------------------------------------------

        [MenuItem("Unity Tools/Scene Select Dropdown")]
        public static void Init()
        {
            SceneSelectDropdown window = EditorWindow.GetWindow<SceneSelectDropdown>( "Scene Select Dropdown" );
            window.minSize = new Vector2( 50.0f, 15.0f );
            window.Show();
        }

        //---------------------------------------------------------------------------------

        protected virtual void OnGUI()
        {
            int current = -1;

            string[] scenePaths;

            if( m_showOnlyScenesInBuild )
            {
                scenePaths = GetScenePaths_AddedToBuild();
            }
            else
            {
                scenePaths = GetScenePaths_AllInProject();
            }

            string[] sceneNames = new string[scenePaths.Length];

            for( int i = 0; i < scenePaths.Length; ++i )
            {
                // Work out the scene name from its path
                int lastSlash = scenePaths[i].LastIndexOf( "/" );
                string name = scenePaths[i].Substring( lastSlash + 1 );
                name = name.Replace( ".unity", "" );

                sceneNames[i] = name;

                if( EditorSceneManager.GetActiveScene().name == name )
                {
                    current = i;
                }
            }

            int newSceneIndex = EditorGUILayout.Popup( current, sceneNames );

            if( newSceneIndex != current )
            {
                bool save = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                if( save )
                {
                    EditorSceneManager.OpenScene( scenePaths[newSceneIndex], OpenSceneMode.Single );
                }
            }

            m_showOnlyScenesInBuild = EditorGUILayout.Toggle( "Scenes In Build Only", m_showOnlyScenesInBuild );
        }

        //---------------------------------------------------------------------------------

        private string[] GetScenePaths_AllInProject()
        {
            string[] sceneAssets = AssetDatabase.FindAssets("t:Scene");

            string[] scenePaths = new string[sceneAssets.Length];

            for( int i = 0; i < sceneAssets.Length; ++i )
            {
                scenePaths[i] = AssetDatabase.GUIDToAssetPath( sceneAssets[i] );
            }

            return scenePaths;
        }

        //---------------------------------------------------------------------------------

        private string[] GetScenePaths_AddedToBuild()
        {
            string[] scenePaths = new string[EditorBuildSettings.scenes.Length];

            for( int i = 0; i < EditorBuildSettings.scenes.Length; ++i )
            {
                scenePaths[i] = EditorBuildSettings.scenes[i].path;
            }

            return scenePaths;
        }

        //---------------------------------------------------------------------------------
    }
}