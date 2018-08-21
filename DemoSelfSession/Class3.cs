using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

public class MySession : Dictionary<string, Object>
{
    /// <summary>
    /// 会话ID
    /// </summary>
    public string SessionID { get; set; }

    public void SetSession(string SessionName, object value)
    {
        LastUpdateTime = DateTime.Now;
        this[SessionName] = value;
    }

    public Object GetSession(string SessionName)
    {
        LastUpdateTime = DateTime.Now;
        Object v = null;
        this.TryGetValue(SessionName, out v);
        return v;
    }

    /// <summary>
    /// 最后一次访问Session的时间
    /// </summary>
    public DateTime LastUpdateTime = DateTime.Now;
}