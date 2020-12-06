using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Client.ServiceReference1;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = new Service1Client();
            int number = 0;
            number = int.Parse(Console.ReadLine());
            var str = client.GetData(number);
            Console.WriteLine(str);
            client.Close();
            Console.ReadLine();
        }
    }
}
