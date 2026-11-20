// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// Author:             Lindsey Keene (NukeAndBeans)
// Contact:            Twitter and Discord: @nukeandbeans
//
// Description:
//      Utility window for viewing built-in editor GUIStyles and icons.
//
// License: MIT
//
// Copyright (c) 2019-2026 Lindsey Keene
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class EditorStyleViewer : EditorWindow {
    private readonly struct BuiltinIcon : IEquatable<BuiltinIcon>, IComparable<BuiltinIcon> {
        public readonly GUIContent icon;
        public readonly GUIContent name;

        public override bool Equals( object o ) {
            return o is BuiltinIcon builtinIcon && Equals( builtinIcon );
        }

        public override int GetHashCode() {
            return name.GetHashCode();
        }

        public bool Equals( BuiltinIcon o ) {
            return name.text == o.name.text;
        }

        public int CompareTo( BuiltinIcon o ) {
            return string.Compare( name.text, o.name.text, StringComparison.Ordinal );
        }

        public BuiltinIcon( GUIContent icon, GUIContent name ) {
            this.icon = icon;
            this.name = name;
        }
    }

    private Vector2 _scrollPosition = new( 0, 0 );
    private string _search = "";
    private string _b64Str = "";
    private Texture2D _texToB64StrSrc;

    private static int m_Tab;

    private int _searchFound;

    private static readonly string tabNumPref = $"{nameof( EditorStyleViewer )}_tabNum";

    private readonly string[] _tabNames = { "Styles", "Icons", "Tools" };

    private static readonly List<GUIStyle> styles = new();
    private static readonly List<BuiltinIcon> icons = new();

    [MenuItem( "Window/Editor Style Viewer" )]
    private static void Init() {
        EditorStyleViewer w = GetWindow<EditorStyleViewer>();
        w.titleContent = new GUIContent( "Editor Style Viewer" );
    }

    private void OnEnable() {
        m_Tab = EditorPrefs.GetInt( tabNumPref, 0 );
    }

    private void OnDisable() {
        EditorPrefs.SetInt( tabNumPref, m_Tab );
    }

    private void OnLostFocus() {
        EditorPrefs.SetInt( tabNumPref, m_Tab );
    }

    [DidReloadScripts]
    private static void OnRecompile() {
        m_Tab = EditorPrefs.GetInt( tabNumPref, 0 );
    }

    private static void RefreshIcons() {
        icons.Clear();

        Texture2D[] t = Resources.FindObjectsOfTypeAll<Texture2D>();

        foreach( Texture2D tex in t ) {
            if( tex.name.Length < 1 ) {
                continue;
            }

            if( tex.hideFlags != HideFlags.HideAndDontSave && tex.hideFlags != (HideFlags.HideInInspector | HideFlags.HideAndDontSave) ) {
                continue;
            }

            if( !EditorUtility.IsPersistent( tex ) ) {
                continue;
            }

            Debug.unityLogger.logEnabled = false;
            GUIContent gc = EditorGUIUtility.IconContent( tex.name );
            Debug.unityLogger.logEnabled = true;

            if( gc == null ) {
                continue;
            }

            if( gc.image == null ) {
                continue;
            }

            icons.Add( new BuiltinIcon( gc, new GUIContent( tex.name ) ) );
        }

        icons.Sort();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    private static void RefreshStyles() {
        styles.Clear();
        styles.AddRange( GUI.skin.customStyles );
    }

    private string ResultsLabel {
        get {
            int count = m_Tab == 0 ? styles.Count : icons.Count;

            if( _searchFound > 0 ) {
                return $"{_searchFound} of {count} {_tabNames[m_Tab].ToLower()} using the term '{_search.ToLower()}'";
            }

            return $"Found {count} {_tabNames[m_Tab].ToLower()}";
        }
    }

    private string RefreshTooltip => $"Refresh {_tabNames[m_Tab].ToLower()} list.\n\nFully reloads the currently selected tab.";

    private static void ExportIcons() {
        for( int i = 0; i < icons.Count; i++ ) {
            ExportIcon( i );
        }
    }

    private static void ExportIcon( int i ) {
        Texture2D tex = EditorGUIUtility.IconContent( icons[i].icon.image.name ).image as Texture2D;
        Texture2D final = new( tex.width, tex.height, tex.format, false );

        Graphics.CopyTexture( tex, 0, 0, final, 0, 0 );

        string path = $"{Directory.GetParent( Application.dataPath.Replace( "/", "\\" ) )?.FullName}\\Icons\\{icons[i].icon.image.name}.png";

        if( !Directory.Exists( $"{Directory.GetParent( Application.dataPath.Replace( "/", "\\" ) )?.FullName}\\Icons\\" ) ) {
            Directory.CreateDirectory( $"{Directory.GetParent( Application.dataPath.Replace( "/", "\\" ) )?.FullName}\\Icons\\" );
        }

        File.WriteAllBytes( path, final.EncodeToPNG() );
        Debug.Log( $"Saved image '{icons[i].icon.image.name}' to '{path}'" );
    }

    private void OnGUI() {
        // only get skins if were actually in the skins tab
        if( m_Tab == 0 ) {
            if( styles.Count < 1 ) {
                RefreshStyles();
            }
        }

        if( m_Tab == 1 ) {
            if( icons.Count < 1 ) {
                RefreshIcons();
            }
        }

        GUILayout.BeginHorizontal( "ToolbarButtonFlat", GUILayout.Height( 28 ) );

        m_Tab = GUILayout.Toolbar( m_Tab, _tabNames, EditorStyles.toolbarButton, GUILayout.Width( 200 ) );
        GUILayout.FlexibleSpace();

        _search = EditorGUILayout.TextField( _search, EditorStyles.toolbarSearchField );

        if( GUILayout.Button( new GUIContent( EditorGUIUtility.IconContent( "d_preAudioLoopOff" ).image, RefreshTooltip ), EditorStyles.toolbarButton ) ) {
            if( m_Tab == 0 ) {
                RefreshStyles();
            }

            if( m_Tab == 1 ) {
                RefreshIcons();
            }
        }

        GUILayout.EndHorizontal();

        if( m_Tab < 2 ) {
            GUILayout.BeginHorizontal( "HelpBox" );
            GUILayout.Label( $"{(m_Tab == 0 ? "Click" : "Double-click")} a sample to copy its name to your clipboard. {(m_Tab == 0 ? "" : "\nIcons are only loaded if their window, gizmo, etc. were previously used or visible.")}", "minilabel" );
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else {
            GUILayout.BeginHorizontal( "HelpBox" );
            GUILayout.Label( $"Select a texture from the project, then click convert to convert it to a base64 string.", "minilabel" );
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        _scrollPosition = GUILayout.BeginScrollView( _scrollPosition, "gameviewbackground" );

        _searchFound = 0;

        Event e = Event.current;

        switch( m_Tab ) {
            case 0: {
                foreach( GUIStyle style in styles ) {
                    if( !style.name.ToLower().Contains( _search.ToLower() ) ) {
                        continue;
                    }

                    GUILayout.BeginHorizontal( "FrameBox" );
                    GUILayout.Space( 7 );

                    if( GUILayout.Button( style.name, style ) ) {
                        EditorGUIUtility.systemCopyBuffer = $"\"{style.name}\"";
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.SelectableLabel( $"\"{style.name}\"" );
                    GUILayout.EndHorizontal();
                    GUILayout.Space( 11 );

                    if( !string.IsNullOrWhiteSpace( _search.ToLower() ) ) {
                        _searchFound++;
                    }
                }

                break;
            }

            case 1: {
                for( int i = 0; i < icons.Count; i++ ) {
                    if( !icons[i].name.text.ToLower().Contains( _search.ToLower() ) ) {
                        continue;
                    }

                    GUILayout.BeginHorizontal( "FrameBox", GUILayout.Height( 24 ) );
                    GUILayout.Space( 16 );

                    EditorGUILayout.LabelField( new GUIContent( icons[i].icon.image, icons[i].name.text ), GUILayout.Height( 24 ) );

                    if( GUILayoutUtility.GetLastRect().Contains( e.mousePosition ) && e.type == EventType.MouseDown && e.clickCount > 1 ) {
                        EditorGUIUtility.systemCopyBuffer = $"{icons[i].name.text}";
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.SelectableLabel( $"\"{icons[i].name.text}\"", GUILayout.Width( 250 ), GUILayout.Height( 24 ) );

                    if( GUILayout.Button( EditorGUIUtility.TrIconContent( "d_DefaultAsset Icon", "Export\n\nExports this icon to the folder 'Icons/' next to 'Assets/'" ), "LargeButtonRight", GUILayout.Height( 24 ), GUILayout.Width( 24 ) ) ) {
                        ExportIcon( i );
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.Space( 2 );

                    if( !string.IsNullOrWhiteSpace( _search.ToLower() ) ) {
                        _searchFound++;
                    }
                }

                break;
            }

            case 2: {
                GUILayout.BeginVertical( "helpbox", GUILayout.ExpandWidth( true ), GUILayout.ExpandHeight( true ) );
                GUILayout.Space( 4 );

                if( _texToB64StrSrc != null ) {
                    GUILayout.BeginHorizontal( "FrameBox" );
                    GUILayout.FlexibleSpace();

                    GUILayout.Label( $"{_texToB64StrSrc.name} [{_texToB64StrSrc.width}x{_texToB64StrSrc.height}, {_texToB64StrSrc.graphicsFormat}]", "CN CountBadge" );
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal( "helpbox", GUILayout.Height( 128 ) );
                EditorGUILayout.LabelField( "Texture", GUILayout.Width( 100 ) );
                _texToB64StrSrc = (Texture2D)EditorGUILayout.ObjectField( _texToB64StrSrc, typeof(Texture2D), false );

                GUILayout.FlexibleSpace();

                if( _texToB64StrSrc != null ) {
                    GUILayout.BeginVertical();
                    GUILayout.Box( _texToB64StrSrc, "helpbox", GUILayout.Width( Mathf.Clamp( _texToB64StrSrc.width, 128, 256 ) ), GUILayout.Height( Mathf.Clamp( _texToB64StrSrc.height, 128, 256 ) ) );
                    GUILayout.EndVertical();
                }
                else {
                    GUILayout.Box( "null", "helpbox", GUILayout.Width( 124 ), GUILayout.Height( 124 ) );
                }

                GUILayout.EndHorizontal();

                GUILayout.Label( "Double-click to copy to clipboard", "helpbox" );
                GUI.enabled = false;
                EditorGUILayout.TextArea( _b64Str, "wordwrappedlabel", GUILayout.ExpandWidth( true ), GUILayout.ExpandHeight( true ) );
                GUI.enabled = true;

                if( GUILayoutUtility.GetLastRect().Contains( e.mousePosition ) ) {
                    if( e.type == EventType.MouseDown && e.button == 0 && e.clickCount > 1 ) {
                        EditorGUIUtility.systemCopyBuffer = _b64Str;
                        ShowNotification( new GUIContent( "Copied to clipboard" ) );
                    }
                }

                GUILayout.EndVertical();

                break;
            }
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal( "HelpBox", GUILayout.Height( 20 ) );

        if( m_Tab == 1 ) {
            if( GUILayout.Button( new GUIContent( "Export Icons", "Export Icons\n\nExports all internal icons (that are currently loaded) to the folder 'Icons/' next to 'Assets/'" ), "minibutton" ) ) {
                ExportIcons();
            }
        }

        if( m_Tab == 2 ) {
            if( GUILayout.Button( "Convert to base64 (Raw Texture Data)" ) ) {
                if( _texToB64StrSrc != null ) {
                    _b64Str = Convert.ToBase64String( _texToB64StrSrc.GetRawTextureData() );
                }
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.Label( ResultsLabel );

        GUILayout.EndHorizontal();
    }
}
