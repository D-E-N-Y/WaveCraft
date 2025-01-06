[System.Serializable]
public struct S_Cost
{
    public E_Resourse resourse;
    public int count;

    public S_Cost(E_Resourse resourse, int count)
    {
        this.resourse = resourse;
        this.count = count;
    }
}