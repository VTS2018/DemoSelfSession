using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace ZGL.Session
{
    public class MySessionContext　
    {
        /// <summary>
        /// 自动销毁过期Session的线程
        /// </summary>
        private static Thread _AutoDestroyTimeOutSessionThread;

        /// <summary>
        /// 销毁超时Session的方法
        /// </summary>
        private static void DestroyTimeOutSession()
        {
            //1.查找过时的Seesion
            Dictionary<string,MySession> SessionPools = MySessionContext.SessionPools;

            while (true)            //无限循环
            {
                Thread.Sleep(60000);//挂起1分钟后再执行 

                //1.查找过时的Seesion 
                DateTime now = DateTime.Now;

                //20分钟的过期时间
                var oldSessions = SessionPools.Where(session => (now - session.Value.LastUpdateTime).Minutes > 20)
                                              .Select(p=>p.Key).ToArray();

                //2.销毁
                foreach (var item in oldSessions)
                {
                    SessionPools.Remove(item);
                }

            }

        }

        /// <summary>
        /// 保存在服务端的Session池,单例且唯一
        /// </summary>
        /// <remarks>
        /// Dictionary<sessionid mysession=""> 
        /// </sessionid></remarks>
        public static readonly Dictionary<string,MySession> SessionPools = new Dictionary<string,MySession>();

        
        static object mLockSeed = new object();
         
        /// <summary>
        /// 取得当前Http请求所对应的MySession,如果本来没有MySession则会自动创建一个MySession.
        /// </summary>
        public static MySession CurrentSession
        {
            get
            {
                HttpContext context = HttpContext.Current;
                MySession mySession = null;
                string SessionId = null;

                //创建超时守护线程.
                if (_AutoDestroyTimeOutSessionThread == null)
                {
                    _AutoDestroyTimeOutSessionThread = new Thread(DestroyTimeOutSession);
                    _AutoDestroyTimeOutSessionThread.Start();
                }

                //1.处理SessionID
                if (  context.Request.Cookies.Count > 0     &&     context.Request.Cookies.Get("MySessionIdentification") != null   )
                {
                    SessionId = context.Request.Cookies.Get("MySessionIdentification").Value;
                }
                 
                if (string.IsNullOrEmpty(SessionId))
                {
                    SessionId = Guid.NewGuid().ToString();

                    //创建一个MySession
                    HttpCookie mySessionCookie = new HttpCookie("MySessionIdentification", SessionId);
                    mySessionCookie.HttpOnly = true;
                    context.Response.AppendCookie(mySessionCookie);
                    
                    //保证在当前的请求中不会重复创建新的MySession 
                    context.Request.Cookies.Add(mySessionCookie); 
                }

                //2.获取SessionID对应Session
                SessionPools.TryGetValue(SessionId, out mySession);

                //3.如果还是没有Session 就新建一个放到Session集合中
                if (mySession == null)
                {
                    lock (SessionPools)
                    {
                        mySession = new MySession();
                        mySession.SessionID = SessionId;
                    
                        SessionPools.Add(SessionId, mySession);
                    }
                    
                } 

                return mySession; 
            }
        }    
    }
}