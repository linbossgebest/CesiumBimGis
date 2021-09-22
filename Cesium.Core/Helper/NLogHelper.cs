using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cesium.Core.Helper
{
    class NLogHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static void Error(object msg, Exception exp = null)
        {
            if (exp == null)
                log.Error(msg);
            else
                log.Error(msg + "\r\n " + exp.ToString());
        }
        public static void Error(string msg)
        {
            log.Error(msg);
        }
        public static void APPError(string msg)
        {
            log.Error(msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">错误描述</param>
        /// <param name="msgf">错误来源</param>
        /// <param name="exp">Exception错误</param>
        public static void Error(object msg, string msgf = "", Exception exp = null)
        {
            if (!string.IsNullOrEmpty(msgf))
            {
                msg = msgf + "\r\n " + msg;
            }
            if (exp == null)
                log.Error(msg);
            else
                log.Error(msg + "\r\n " + exp.ToString());
        }
        public static void Debug(object msg, Exception exp = null)
        {
            if (exp == null)
                log.Debug(msg);
            else
                log.Debug(msg + "\r\n " + exp.ToString());
        }
        public static void Debug(string msg)
        {
            log.Debug(msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">错误描述</param>
        /// <param name="msgf">错误来源</param>
        /// <param name="exp">Exception错误</param>
        public static void Debug(object msg, string msgf = "", Exception exp = null)
        {
            if (!string.IsNullOrEmpty(msgf))
            {
                msg = msgf + "\r\n " + msg;
            }
            if (exp == null)
                log.Debug(msg);
            else
                log.Debug(msg + "\r\n " + exp.ToString());
        }
        //public static void Info(string msg)
        //{
        //    if (!islog) return;
        //    log.Info(msg);
        //}
        public static void Info(string msg, Exception exp = null)
        {

            if (exp == null)
                log.Info(msg);
            else
                log.Info(msg + "\r\n " + exp.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">错误描述</param>
        /// <param name="msgf">错误来源</param>
        /// <param name="exp">Exception错误</param>
        public static void Info(object msg, string msgf = "", Exception exp = null)
        {
            if (!string.IsNullOrEmpty(msgf))
            {
                msg = msgf + "\r\n " + msg;
            }
            if (exp == null)
                log.Info(msg);
            else
                log.Info(msg + "\r\n " + exp.ToString());
        }
        public static void Warn(string msg)
        {
            log.Warn(msg);
        }
        public static void Warn(object msg, Exception exp = null)
        {
            if (exp == null)
                log.Warn(msg);
            else
                log.Warn(msg + "\r\n " + exp.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">错误描述</param>
        /// <param name="msgf">错误来源</param>
        /// <param name="exp">Exception错误</param>
        public static void Warn(object msg, string msgf = "", Exception exp = null)
        {
            if (!string.IsNullOrEmpty(msgf))
            {
                msg = msgf + "\r\n " + msg;
            }
            if (exp == null)
                log.Warn(msg);
            else
                log.Warn(msg + "\r\n " + exp.ToString());
        }

    }
}
