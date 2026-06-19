using System;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEngine.Object), true)]
[CanEditMultipleObjects]
public class EditorButtonDrawer : Editor
{
    private class EditorButtonState
    {
        public bool opened;
        public System.Object[] Parameters;

        public EditorButtonState(int numberOfParameters)
        {
            Parameters = new object[numberOfParameters];
        }
    }

    private EditorButtonState[] editorButtonStates;

    private delegate object ParameterDrawer(ParameterInfo parameter, object val);

    private readonly Dictionary<Type, ParameterDrawer> m_typeDrawer = new()
    {
        { typeof(float), DrawFloatParameter },
        { typeof(int), DrawIntParameter },
        { typeof(Enum), DrawIntParameter },
        { typeof(string), DrawStringParameter },
        { typeof(bool), DrawBoolParameter },
        { typeof(Color), DrawColorParameter },
        { typeof(Vector3), DrawVector3Parameter },
        { typeof(Vector2), DrawVector2Parameter },
        { typeof(Quaternion), DrawQuaternionParameter }
    };

    private readonly Dictionary<Type, string> m_typeDisplayName = new()
    {
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(string), "string" },
        { typeof(bool), "bool" },
        { typeof(Color), "Color" },
        { typeof(Vector3), "Vector3" },
        { typeof(Vector2), "Vector2" },
        { typeof(Quaternion), "Quaternion" }
    };

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IEnumerable<MemberInfo> methods = target.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                        BindingFlags.NonPublic)
            .Where(o => Attribute.IsDefined(o, typeof(EditorButton)));

        int methodIndex = 0;

        if (editorButtonStates == null)
        {
            CreateEditorButtonStates(methods.Select(member => (MethodInfo)member).ToArray());
        }

        foreach (MemberInfo memberInfo in methods)
        {
            MethodInfo method = memberInfo as MethodInfo;
            EditorButton editorButton = method.GetCustomAttribute<EditorButton>();
            if (editorButton.ShouldDrawButton())
            {
                DrawButtonForMethod(new[] { target }, method, GetEditorButtonState(method, methodIndex));
            }
            methodIndex++;
        }
    }

    private void CreateEditorButtonStates(MethodInfo[] methods)
    {
        editorButtonStates = new EditorButtonState[methods.Length];
        int methodIndex = 0;
        foreach (MethodInfo methodInfo in methods)
        {
            editorButtonStates[methodIndex] = new EditorButtonState(methodInfo.GetParameters().Length);
            methodIndex++;
        }
    }

    private EditorButtonState GetEditorButtonState(MethodInfo method, int methodIndex)
    {
        return editorButtonStates[methodIndex];
    }

    private void DrawButtonForMethod(object[] invokationTargets, MethodInfo methodInfo, EditorButtonState state)
    {
        EditorGUILayout.BeginHorizontal();
        if (state.Parameters.Length > 0)
        {
            Rect foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10));
            state.opened = EditorGUI.Foldout(foldoutRect, state.opened, "");
        }
        else
        {
            GUILayout.Space(13);
        }

        bool clicked = GUILayout.Button(new GUIContent(ObjectNames.NicifyVariableName(methodInfo.Name), MethodDisplayName(methodInfo)),
            GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        if (state.opened)
        {
            using (new GUILayout.VerticalScope("Box"))
            {
                EditorGUI.indentLevel++;
                int paramIndex = 0;
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                {
                    object currentVal = state.Parameters[paramIndex];
                    state.Parameters[paramIndex] = DrawParameterInfo(parameterInfo, currentVal);
                    paramIndex++;
                }

                EditorGUI.indentLevel--;
            }
        }

        if (!clicked)
        {
            return;
        }

        foreach (object invokationTarget in invokationTargets)
        {
            
            object returnVal = methodInfo.Invoke(invokationTarget, state.Parameters);

            if (returnVal is IEnumerator && invokationTarget is MonoBehaviour monoTarget)
            {
                monoTarget.StartCoroutine((IEnumerator)returnVal);
            }
            else if (returnVal != null)
            {
                Debug.Log($"Method call result -> {returnVal}");
            }
        }
    }

    private object GetDefaultValue(ParameterInfo parameter)
    {
        bool hasDefaultValue = !DBNull.Value.Equals(parameter.DefaultValue);

        if (hasDefaultValue)
            return parameter.DefaultValue;

        Type parameterType = parameter.ParameterType;
        if (parameterType.IsValueType)
            return Activator.CreateInstance(parameterType);

        return null;
    }

    private object DrawParameterInfo(ParameterInfo parameterInfo, object currentValue)
    {
        object paramValue = null;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(parameterInfo.Name);

        ParameterDrawer drawer = GetParameterDrawer(parameterInfo);
        if (currentValue == null)
            currentValue = GetDefaultValue(parameterInfo);
        paramValue = drawer.Invoke(parameterInfo, currentValue);

        EditorGUILayout.EndHorizontal();

        return paramValue;
    }

    private ParameterDrawer GetParameterDrawer(ParameterInfo parameter)
    {
        Type parameterType = parameter.ParameterType;

        if (typeof(UnityEngine.Object).IsAssignableFrom(parameterType))
            return DrawUnityEngineObjectParameter;

        ParameterDrawer drawer;
        if (m_typeDrawer.TryGetValue(parameterType, out drawer))
        {
            return drawer;
        }

        if (m_typeDrawer.TryGetValue(parameterType.BaseType, out drawer))
        {
            return drawer;
        }

        return null;
    }

    private static object DrawFloatParameter(ParameterInfo parameterInfo, object val)
    {
        //Since it is legal to define a float param with an integer default value (e.g void method(float p = 5);)
        //we must use Convert.ToSingle to prevent forbidden casts
        //because you can't cast an "int" object to float
        //See for http://stackoverflow.com/questions/17516882/double-casting-required-to-convert-from-int-as-object-to-float more info

        return EditorGUILayout.FloatField(Convert.ToSingle(val));
    }

    private static object DrawIntParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.IntField((int)val);
    }

    private static object DrawBoolParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Toggle((bool)val);
    }

    private static object DrawStringParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.TextField((string)val);
    }

    private static object DrawColorParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.ColorField((Color)val);
    }

    private static object DrawUnityEngineObjectParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.ObjectField((UnityEngine.Object)val, parameterInfo.ParameterType, true);
    }

    private static object DrawVector2Parameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Vector2Field("", (Vector2)val);
    }

    private static object DrawVector3Parameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Vector3Field("", (Vector3)val);
    }

    private static object DrawQuaternionParameter(ParameterInfo parameterInfo, object val)
    {
        return Quaternion.Euler(EditorGUILayout.Vector3Field("", ((Quaternion)val).eulerAngles));
    }

    private string MethodDisplayName(MethodInfo method)
    {
        StringBuilder sb = new();
        sb.Append($"{method.Name}(");
        ParameterInfo[] methodParams = method.GetParameters();
        foreach (ParameterInfo parameter in methodParams)
        {
            sb.Append(MethodParameterDisplayName(parameter));
            sb.Append(",");
        }

        if (methodParams.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        sb.Append(")");
        return sb.ToString();
    }

    private string MethodParameterDisplayName(ParameterInfo parameterInfo)
    {
        string parameterTypeDisplayName;
        if (!m_typeDisplayName.TryGetValue(parameterInfo.ParameterType, out parameterTypeDisplayName))
        {
            parameterTypeDisplayName = parameterInfo.ParameterType.ToString();
        }

        return $"{parameterTypeDisplayName} {parameterInfo.Name}";
    }

    private string MethodUID(MethodInfo method)
    {
        StringBuilder sb = new();
        sb.Append($"{method.Name}_");
        foreach (ParameterInfo parameter in method.GetParameters())
        {
            sb.Append(parameter.ParameterType);
            sb.Append("_");
            sb.Append(parameter.Name);
        }

        sb.Append(")");
        return sb.ToString();
    }
}
#endif
