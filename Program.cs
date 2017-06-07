using IBApi;
using Samples;
using System;
using System.Threading;

namespace IBApiExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ibClient = new EWrapperImpl();

            Console.WriteLine("Connecting...");

            ibClient.ClientSocket.eConnect("127.0.0.1", 4002, 0);

            //Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
            reader.Start();
            //Once the messages are in the queue, an additional thread can be created to fetch them
            new Thread(() => {
                while (ibClient.ClientSocket.IsConnected())
                { ibClient.Signal.waitForSignal();
                    reader.processMsgs(); }
            }) { IsBackground = true }.Start();

            Contract contract = new Contract();
            contract.Symbol = "USD";
            contract.SecType = "CASH";
            contract.Currency = "CAD";
            contract.Exchange = "IDEALPRO";

            Console.WriteLine("Requesting Data...");

            ibClient.ClientSocket.reqMktData(1, contract, "", false, false, null);
            Thread.Sleep(1000);
            ibClient.ClientSocket.reqRealTimeBars(1, contract, 5, "MIDPOINT", true, null);
            Thread.Sleep(1000);

            Console.WriteLine("Press any key to exit");
            Console.Read();

            ibClient.ClientSocket.eDisconnect();
        }
    }
}
