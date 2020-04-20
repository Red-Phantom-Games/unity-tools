
using UnityEngine;
using UnityEditor;

namespace UnityTools
{
    public class ClipboardToAssetNames : MonoBehaviour
    {
        //---------------------------------------------------------------------------------

        [MenuItem( "Assets/Paste Names", true )]
        private static bool ClipboardToAssetNames_Validate()
        {
            // Get list of selected assets in project window
            Object[] objects = Selection.GetFiltered<Object>( SelectionMode.Assets );

            // We allow the paste if there's at least one asset selected and the clipboard has some string in it
            return (objects.Length != 0 && GUIUtility.systemCopyBuffer != "");
        }

        //---------------------------------------------------------------------------------

        [MenuItem( "Assets/Paste Names", false )]
        private static void ClipboardToAssetNames_Do()
        {
            // Convert clipboard into an array of strings to be used as asset names
            string[] filter = { System.Environment.NewLine };
            string[] entries = GUIUtility.systemCopyBuffer.Split( filter, System.StringSplitOptions.RemoveEmptyEntries );

            // Get list of selected assets in project window
            Object[] objects = Selection.GetFiltered<Object>( SelectionMode.Assets );

            bool proceedWithRenaming = true;

            // Throw a dialog warning if the number of selected assets isn't the same as what we've got from the clipboard
            if( objects.Length != entries.Length )
            {
                proceedWithRenaming = EditorUtility.DisplayDialog( "Renaming Assets From Clipboard Warning!", "Number of clipboard entires differ from number of selected assets. Proceed?", "Continue", "Cancel" );
            }

            // Continue with renaming the assets if allowed
            if( proceedWithRenaming )
            {
                int entryIndex = 0;

                foreach( Object obj in objects )
                {
                    // Quit if we run out of names from the clipboard
                    if( entryIndex >= entries.Length )
                    {
                        break;
                    }

                    // Assuming asset path of a selected object is always valid
                    string assetPath = AssetDatabase.GetAssetPath( obj );

                    // Rename asset to name from from clipboard
                    AssetDatabase.RenameAsset( assetPath, entries[entryIndex] );
                    ++entryIndex;
                }
            }
        }

        //---------------------------------------------------------------------------------
    }
}