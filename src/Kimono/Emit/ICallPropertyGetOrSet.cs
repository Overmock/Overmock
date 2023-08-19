namespace Kimono.Emit
{
    /// <summary>
    /// Interface ICallPropertGetOrSetEmitter
    /// Extends the <see cref="ICallPropertyGet" />
    /// Extends the <see cref="ICallPropertySet" />
    /// </summary>
    /// <seealso cref="ICallPropertyGet" />
    /// <seealso cref="ICallPropertySet" />
    public interface ICallPropertyGetOrSet : ICallPropertyGet, ICallPropertySet
    {
    }

    /// <summary>
    /// Interface ICallPropertGetOrSetEmitter
    /// Extends the <see cref="ICallPropertyGet{TDelegate}" />
    /// Extends the <see cref="ICallPropertySet{TDelegate}" />
    /// </summary>
    /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
    /// <seealso cref="ICallPropertyGet{TDelegate}" />
    /// <seealso cref="ICallPropertySet{TDelegate}" />
    public interface ICallPropertyGetOrSet<TDelegate> : ICallPropertyGet<TDelegate>, ICallPropertySet<TDelegate> where TDelegate : Delegate
    {
    }
}
