using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace WcfService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Service1 : IService1
    {
        
        protected List<object> AppendCol;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void ReadColumns(string[] columns)
        {
            if(AppendCol !=null)
            {
                AppendCol = new List<object>();
            }
            var count = columns.Length;
            for(int i=0;i<count;i++)
            {
                AppendCol.Add(columns[i]);
                Console.WriteLine(columns[i]);
            }
        }

        public void AppendDataInDB(string s)
        {
            string[] AppendColumns = s.Split('|');
            string ds = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Companies; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            var con = new SqlConnection(ds);
            con.Open();            
            var command = new SqlCommand(string.Format(
                     @"declare @count int
                        select @count = count(ID) FROM [dbo].[Town] where ID={0}
                        if (@count=0)
                        insert into [Town] VALUES ({0},'{1}','{2}')
                        select @count = count(ID) FROM [dbo].[Company] where ID={3}
                        if (@count=0)
                        insert into [dbo].[Company] VALUES({3}, '{4}', '{5}', {0})
                        select @count = count(ID) FROM dbo.Branch where ID={6}
                        if (@count=0)
                        insert into dbo.Branch VALUES({6}, '{7}', {8}, {3})
                        select @count = count(ID) FROM dbo.Post where ID={11}
                        if (@count=0)
                        insert into dbo.Post VALUES({11}, '{12}')
                        select @count = count(ID) FROM dbo.Person where ID={9}
                        if (@count=0)
                        insert into dbo.Person VALUES({9}, '{10}', {6}, {11})
                        ",
                        AppendColumns[0], AppendColumns[1], AppendColumns[2], AppendColumns[3],
                        AppendColumns[4], AppendColumns[5], AppendColumns[6], AppendColumns[7],
                        AppendColumns[8], AppendColumns[9], AppendColumns[10],
                        AppendColumns[11], AppendColumns[12]),
                con);
            command.ExecuteReader().Read();
            con.Close();
            //AppendColumns.Clear();
        }                     
    }
}
