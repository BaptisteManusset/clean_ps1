using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButton : PropertyAttribute
{
    #region Properties
    private readonly EEditorPlayMode m_playMode;
    public EEditorPlayMode PlayMode => m_playMode;
    #endregion

    public EditorButton() : this(EEditorPlayMode.PlayModeAndEditor)
    {
    }

    public EditorButton(bool a_drawOnlyInPlayMode) : this(a_drawOnlyInPlayMode ?
        EEditorPlayMode.PlayModeOnly : EEditorPlayMode.PlayModeAndEditor)
    {
    }

    public EditorButton(EEditorPlayMode a_playMode)
    {
        m_playMode = a_playMode;
    }

    public bool ShouldDrawButton()
    {
        switch (m_playMode)
        {
            case EEditorPlayMode.PlayModeAndEditor:
            default:
                return true;
            case EEditorPlayMode.PlayModeOnly:
                return Application.isPlaying;
            case EEditorPlayMode.EditorOnly:
                return !Application.isPlaying;
        }
    }
}
