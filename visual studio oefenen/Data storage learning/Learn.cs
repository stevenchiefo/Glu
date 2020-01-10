using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_learning
{
    public class Learn
    {
        private List<string[]> m_Strings = new List<string[]>();

        public Learn()
        {
            Task();
        }

        private void Task()
        {
            string[] hello = { "hi", "hola", "hallo" };
            string[] doei = { "Bye", "Doei", "Flikker op" };
            string[] naam = { "Sven", "Jur", "Floris" };
            m_Strings.Add(hello);
            m_Strings.Add(doei);
            m_Strings.Add(naam);
            foreach (string[] i in m_Strings)
            {
                foreach (string v in i)
                {
                    Console.WriteLine(v);
                }
            }
            Console.ReadKey();
        }
    }
}