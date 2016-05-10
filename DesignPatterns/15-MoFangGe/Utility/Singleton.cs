namespace Climb.Utility
{
    /// <summary>
    /// Singleton泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // ReSharper disable once ClassCannotBeInstantiated
    public sealed class Singleton<T> where T : new()
    {
        private static T _instance = new T();

        // ReSharper disable once StaticMemberInGenericType
        static readonly object LockHelper = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        private Singleton()
        { }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static T GetInstance()
        {
            if (_instance != null) return _instance;
            lock (LockHelper)
            {
                if (_instance == null)
                {
                    // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                    _instance = new T();
                }
            }

            return _instance;
        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="value"></param>
        public void SetInstance(T value)
        {
            _instance = value;
        }

    }
}
