namespace SameApp.Models;

public class TokenData
{
    // Dict save users that login from android app.
    private static TokenData _tokenData;
    private static Dictionary<string, string> _tokens = null;

    private TokenData()
    {
        _tokens = new Dictionary<string, string>();
    }

    public static TokenData GetInstance()
    {
        if (_tokenData == null)
        {
            _tokenData = new TokenData();
        }

        return _tokenData;
    }

    public Dictionary<string, string> GetTokens()
    {
        return _tokens;
    }

    public void AddKey(string userName, string token)
    {
        _tokens.Add(userName, token);
    }

    public void PushNotification(string receiver, string sender, string content)
    {
        string token = _tokens[receiver];

    }

}