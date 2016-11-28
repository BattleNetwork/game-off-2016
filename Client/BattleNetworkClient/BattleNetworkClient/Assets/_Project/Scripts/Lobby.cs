public class Lobby
{
    private string _name;
    private int _nbPlayer;

    public Lobby(string name, int nbPlayer)
    {
        Name = name;
        NbPlayer = nbPlayer;
    }

    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }

    public int NbPlayer
    {
        get
        {
            return _nbPlayer;
        }

        set
        {
            _nbPlayer = value;
        }
    }
}