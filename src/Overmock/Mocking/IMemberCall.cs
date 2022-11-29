namespace Overmock.Mocking
{
    public interface IMemberCall : IVerifiable
    {
        void Throws(Exception exception);

        void Returns(Func<object> valueProvider);

        IEnumerable<MemberOverride> GetOverrides();
    }
}