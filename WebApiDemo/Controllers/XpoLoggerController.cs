using DevExpress.Xpo.Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    public class XpoLoggerController : ControllerBase
    {
        readonly static LoggerBase logger = new LoggerBase(50000);
        static XpoLoggerController()
        {
            LogManager.SetTransport(logger);
        }
        [HttpGet]
        public LogMessage[] GetCompleteLog()
        {
            return logger.GetCompleteLog();
        }
        [HttpGet]
        public LogMessage GetMessage()
        {
            return logger.GetMessage();
        }
        [HttpGet]
        public LogMessage[] GetMessages(int messagesAmount)
        {
            return logger.GetMessages(messagesAmount);
        }
    }
}
