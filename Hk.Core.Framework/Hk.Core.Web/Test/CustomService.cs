using System;

namespace Hk.Core.Web.Test
{
    public class CustomService : ICustomService
    {
        public void Call(string id)
        {
            Console.WriteLine($"service calling...{id}");
        }
    }
}