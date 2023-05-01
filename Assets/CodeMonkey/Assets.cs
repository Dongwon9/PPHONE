/*
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;

namespace CodeMonkey {
    /*
     * Global Asset references
     * Edit Asset references in the prefab CodeMonkey/Resources/CodeMonkeyAssets
     * */

    public class Assets : MonoBehaviour {

        // Instance reference
        public static Assets i {
            get {
                if (_i == null)
                    _i = Instantiate(Resources.Load<Assets>("CodeMonkeyAssets"));
                return _i;
            }
        }

        public Material m_White;
        public Sprite s_Circle;
        public Sprite s_White;
        // Internal instance reference
        private static Assets _i;
        // All references
    }
}