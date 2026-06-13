using TMPro;

public class TodoListUI : SceneSingleton<TodoListUI>
{
    public TMP_Text Trash;

    public void SetText(string text)
    {

        Trash.text = text;
    }
}