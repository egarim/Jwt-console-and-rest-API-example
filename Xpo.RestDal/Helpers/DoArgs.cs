using System;

namespace Xpo.RestDataStore
{
    public class DoArgs
    {

        public string Command { get; set; }
        public object Parameters { get; set; }
        public DoArgs(string command, object parameters)
        {
            Command = command;
            Parameters = parameters;
        }

        public DoArgs()
        {
        }
    }
}