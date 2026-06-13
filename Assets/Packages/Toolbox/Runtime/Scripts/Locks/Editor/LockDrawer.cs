using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Lock))]
public class LockDrawer : PropertyDrawer
{
    private const string StateName = "LockShowMore";

    private static object m_toRemove = null;

    private Lock m_lock;

    private static bool m_showMore
    {
        get => EditorPrefs.GetBool(StateName, false);
        set => EditorPrefs.SetBool(StateName, value);
    }


    [RuntimeInitializeOnLoadMethod]
    public static void InitUdr()
    {
        m_toRemove = null;
    }

    public override void OnGUI(Rect a_position, SerializedProperty a_property, GUIContent a_label)
    {
        m_lock = fieldInfo.GetValue(a_property.serializedObject.targetObject) as Lock;

        if (m_lock == null)
        {
            EditorGUILayout.LabelField(a_label, new GUIContent("null"));
            return;
        }

        DrawHeader(a_label);

        if (!m_lock.IsUnlocked)
        {
            DrawFoldout();
        }
    }

    private void DrawHeader(GUIContent a_label)
    {
        string contentText = m_lock.IsLocked ? "Locked: " : "Unlock";
        if (m_lock.IsSelfLocked) contentText += "Self";
        if (m_lock.IsSelfLocked && m_lock.IsExternallyLocked) contentText += " and ";
        if (m_lock.IsExternallyLocked) contentText += $"{m_lock.ExternalLocks.Count} External";

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label(a_label, GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.Label(new GUIContent(contentText), EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();

            if (m_lock.IsLocked &&
                GUILayout.Button(
                    EditorGUIUtility.IconContent(m_showMore ? "scenevis_visible_hover" : "scenevis_hidden_hover"),
                    GUILayout.Width(20)))
            {
                m_showMore = !m_showMore;
            }

            if (!m_lock.ExternalLocks.Contains(this) && GUILayout.Button("+", GUILayout.Width(20)))
            {
                m_lock.AddExternalLock(this);
            }
        }
    }

    private void DrawFoldout()
    {
        if (!m_showMore) return;
        EditorGUI.indentLevel++;

        using (new GUILayout.VerticalScope("box"))
        {
            if (m_lock.IsLocked)
            {
                EditorGUILayout.LabelField("Self", m_lock.IsSelfLocked ? "Locked" : "None");
                EditorGUILayout.LabelField(
                    "External", $"{(m_lock.IsExternallyLocked ? $"{m_lock.ExternalLocks.Count} Locked" : "None")}");
                if (m_lock.IsExternallyLocked)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Space(EditorGUIUtility.labelWidth);

                        using (new EditorGUILayout.VerticalScope())
                        {
                            foreach (object externalLock in m_lock.ExternalLocks)
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.LabelField($"- {externalLock}");
                                    if (GUILayout.Button(new GUIContent("X", "Remove Lock"), GUILayout.Width(20)))
                                    {
                                        m_toRemove = externalLock;
                                    }
                                }
                            }

                            if (m_toRemove != null)
                            {
                                m_lock.RemoveExternalLock(m_toRemove);
                                m_toRemove = null;
                            }
                        }
                    }
                }
            }
        }

        EditorGUI.indentLevel--;
    }
}