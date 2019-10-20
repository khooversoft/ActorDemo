namespace Customer_Management
{
    public interface IOption
    {
        int CustomerCount { get; }

        bool Deploy { get; }
    }
}