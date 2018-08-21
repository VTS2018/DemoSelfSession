using System;
using ZGL.Session;

namespace ZGL.Common
{
    /**
     * 本想把 MySessionContext.CurrentSession做成静态的变量好提高程序速度.但是后来一想不行..
     * 静态的对象任何一个人访问的都是同一个对象...这样到时候Session就乱了..
     * 想来想去没有更好的提高效率的方法.只能暂时先这么用着..效率估计也不低..
     */
    /// <summary>
    /// Session助手类.
    /// </summary>
    public static class SessionHelper
    {

        /// <summary>
        /// 移除seesion中指定的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void RemveSession(string name)
        {
            if (IsNull(name))
            {
                return;
            }
            MySessionContext.CurrentSession.Remove(name);
        }


        /// <summary>
        /// 获取seesion中指定的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSession(string name)
        {
            if (IsNull(name))
            {
                return "";
            }
            return MySessionContext.CurrentSession.GetSession(name).ToString();
        }

        /// <summary>
        /// 获取seesion中指定的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetObjectSession(string name)
        {
            if (IsNull(name))
            {
                return null;
            }
            return MySessionContext.CurrentSession.GetSession(name);
        }

        /// <summary>
        /// 获取seesion中指定的int值
        /// </summary>
        /// <param name="name"></param>
        /// <returns>如果没有值返回-1.</returns>
        public static int GetIntSession(string name)
        {
            if (IsNull(name))
            {
                return -1;
            }
            return Convert.ToInt32(MySessionContext.CurrentSession.GetSession(name));
        }

        /// <summary>
        /// 设置seesion中指定的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetSession(string name, object value)
        {
            if (MySessionContext.CurrentSession == null)
            {
                return false;
            }
            MySessionContext.CurrentSession.SetSession(name, value);
            return true;
        }

        /// <summary>
        /// 判断Session中某个变量是否为null或""
        /// </summary>
        /// <param name="name">session中的变量名</param>
        /// <returns></returns>
        public static bool SessionIsNullOrEmpty(string name)
        {
            if (IsNull(name))
            {
                return true;
            }

            if (MySessionContext.CurrentSession.GetSession(name) == "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检测是不是为空.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsNull(string name)
        {
            if (MySessionContext.CurrentSession.GetSession(name) == null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 判断Session中某个变量是否有值 
        /// </summary>
        /// <param name="name">session中的变量名</param>
        /// <returns></returns>
        public static bool SessionHasValue(string name)
        {
            return !SessionIsNullOrEmpty(name);
        }
    }
}