public class MainMenuModel
{
    private bool _isDirty;
    private string _status;

    public bool IsDirty
    {
        get { return _isDirty; }
        set { _isDirty = value; }
    }

    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public MainMenuModel()
    {
        _isDirty = false;
        _status = "login";
    }
}