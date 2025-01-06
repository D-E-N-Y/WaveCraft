[System.Serializable]
public struct S_Cost
{
    public E_Resource resourse;
    public int count;

    public S_Cost(E_Resource resourse, int count)
    {
        this.resourse = resourse;
        this.count = count;
    }
}