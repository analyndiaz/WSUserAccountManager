namespace WSUserAccountManager.Abstractions
{
    public interface IHashFunction
    {
        string GetHashValue(string secretKey, string message);
    }
}
