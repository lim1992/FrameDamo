using System;


namespace ETModel
{
    public abstract class ServiceModule<T> : Module where T : ServiceModule<T>, new()
    {
        private static T ms_instance = default(T);

        /// <summary>
        /// 用于实现单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    ms_instance = new T();
                }

                return ms_instance;
            }
        }

        /// <summary>
        /// 调用它以创建模块
        /// 并且检查它是否以单例形式创建
        /// </summary>
        /// <param name="args"></param>
        protected void CheckSingleton()
        {
            if (ms_instance == null)
            {
                var exp = new Exception("ServiceModule<" + typeof(T).Name + "> 无法直接实例化，因为它是一个单例!");
                throw exp;
            }
        }
    }
}
