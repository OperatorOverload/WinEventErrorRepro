using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinEventErrorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadEvents();
        }

        public static void ReadEvents()
        {
            String logName = "Application";

            String queryString = "*[System/Level=3]";

            EventLogQuery eventsQuery = new EventLogQuery(logName,
                PathType.LogName, queryString);

            EventLogReader logReader;

            try
            {
                // Query the log and create a stream of selected events
                logReader = new EventLogReader(eventsQuery);
            }
            catch (EventLogNotFoundException e)
            {
                Console.WriteLine("Failed to query the log!");
                Console.WriteLine(e);
                return;
            }

            // For each event returned from the query
            for (EventRecord eventInstance = logReader.ReadEvent();
                    eventInstance != null;
                    eventInstance = logReader.ReadEvent())
            {
                //build an IEnumerable<object> 
                List<object> varRepSet = new List<object>();
                for (int i = 0; i < eventInstance.Properties.Count; i++)
                {
                    varRepSet.Add((object)(eventInstance.Properties[i].Value.ToString()));
                }

                //wrapped in a try block as some fail to resolve using the second pathway (provider not found error"
                try
                {
                    //WORKS
                    string description1 = eventInstance.FormatDescription();

                    //neither of these work, they bring back the template string with empty values substituted
                    //BROKEN: format description with the built-in properties array
                    string description2 = eventInstance.FormatDescription(eventInstance.Properties);
                    //BROKEN: format description with the input explicitly typed as an IEnumerable<object>
                    string description3 = eventInstance.FormatDescription(varRepSet.AsEnumerable());

                    Console.WriteLine(description1);
                    Console.WriteLine(description2);
                    Console.WriteLine(description3);
                    Console.WriteLine("PRESS ANY KEY FOR NEXT EVENT, or CTRL+C TO EXIT...");
                    Console.ReadKey();
                    

                }
                catch (Exception)
                {

                }
            }
            
        }

       

    }
}
