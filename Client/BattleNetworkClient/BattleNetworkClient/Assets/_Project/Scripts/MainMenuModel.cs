public class MainMenuModel
{
    private bool _isDirty;
    private string _status;
    private string _errorMessage;

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

    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }

    public MainMenuModel()
    {
        _isDirty = false;
        _status = "login";
        _errorMessage = "";
    }
}