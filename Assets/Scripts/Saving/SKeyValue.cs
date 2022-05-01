[System.Serializable]
public class SKeyValue<T>
{
    public string key;
    public T value;

    public SKeyValue(string key, T value)
    {
        this.key = key;
        this.value = value;
    }
}
