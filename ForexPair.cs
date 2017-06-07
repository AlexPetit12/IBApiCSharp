using log4net;
using log4net.Appender;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBApiExample
{
    public class ForexPair
    {
        string Symbol { get; set; }

        ILog log { get; set; }

        public ForexPair(string symbol)
        {
            Symbol = symbol;
            InitLog();
        }

        public void InitLog()
        {
            FileAppender fileAppender = new FileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();

            var date = DateTime.Now.ToShortDateString();
            fileAppender.File = "C:\\Users\\Alexandre\\Desktop\\Forex\\LogsCSharp\\" + date + "\\" + Symbol + ".txt";
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = "%d [%2%t] %-5p [%-10c]   %m%n%n";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(fileAppender);

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
