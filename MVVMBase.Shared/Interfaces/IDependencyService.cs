namespace MVVMBase.Interfaces
{
    /// <summary>
    ///     Interface to wrap a ServiceLocator
    /// </summary>
    public interface IDependencyService
    {
        /// <summary>
        ///     Register a specific type as an abstraction
        /// </summary>
        /// <typeparam name="T">The class to register</typeparam>
        void Register<T>() where T : class, new();

        /// <summary>
        ///     Register a specific abstraction associated to a type.
        /// </summary>
        /// <typeparam name="T">The abstraction</typeparam>
        /// <typeparam name="TImpl">The implementation</typeparam>
        void Register<T, TImpl>() where T : class where TImpl : class, T, new();

        /// <summary>
        ///     Register a specific instance of an abstraction.
        /// </summary>
        /// <typeparam name="T">Abstraction type</typeparam>
        /// <param name="impl">Instance to use</param>
        void Register<T>(T impl) where T : class;

        /// <summary>
        ///     Retrieve a specific implementation from the locator.
        /// </summary>
        /// <typeparam name="T">Type to look for</typeparam>
        T Get<T>() where T : class;
    }
}