namespace PortfolioTracker.Core.Markers
{
    public interface IValueObject
    {
        bool IsSameAs(object other);
    }
}
