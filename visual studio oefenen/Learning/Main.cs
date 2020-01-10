using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Learning
{
    internal class Main
    {
        private Dictionary<string, int> m_Inventory = new Dictionary<string, int>();
        private string m_FilePath = "Hentai.txt";
        private string m_Oplag;
        private int m_Level = 0;
        private int m_Gebied = 0;

        public Main()
        {
            Load();
            Start();
            WachtOpKey();
        }

        private void Start()
        {
            m_Inventory.Add("One", 1);
            m_Inventory.Add("two", 2);
            m_Inventory.Add("Three", 3);

            foreach (KeyValuePair<string, int> items in m_Inventory)
            {
                Console.WriteLine(items.Key + " " + items.Value);
            }
            try
            {
                Console.WriteLine(m_Oplag);
            }
            catch
            {
                Console.WriteLine("File not Found");
            }
            Random rand = new Random();
            int Num = rand.Next(0, 20);
            Console.WriteLine("Je sorce is " + Num);
            m_Oplag = Num.ToString();
            Console.WriteLine("Score word nu gesaved");
            Save();
        }

        private void WachtOpKey()
        {
            m_Inventory.Select(c => c.Key).ToArray();
            List<string> names = new List<string>()
            {
                "Steven",
                "Luke",
                "Milan"
            };

            string[] nNames = names.Where(n => n.Contains("n")).Select(n => n.ToLower()).ToArray();

            Console.WriteLine("Click op een toets");
            Console.ReadKey();
        }

        public string ToHigher(string str)
        {
            return str.ToUpper();
        }

        private void Save()
        {
            m_Oplag = "Level" + m_Level + "\n" + "Gebied" + m_Gebied;

            if (File.Exists(m_FilePath))
            {
                using (FileStream file = File.Open(m_FilePath, FileMode.Open))
                {
                    //File.OpenWrite(m_FilePath);
                    //File.WriteAllText(m_FilePath,);
                    using (StreamWriter Writer = new StreamWriter(m_FilePath, true)) //boolean is voor het toevoegen van een line (append)
                    {
                        Writer.WriteLine("Yo");
                    }
                }
            }
            else
            {
                File.Create(m_FilePath);
                File.WriteAllText(m_FilePath, m_Oplag);
                File.OpenRead(m_FilePath);
            }
        }

        private void Load()
        {
            using (FileStream file = File.Open(m_FilePath, FileMode.Open))
            {
                using (StreamReader Reader = new StreamReader(m_FilePath))
                {
                    while (Reader.Peek() > 0)
                    {
                    }
                }
            }
        }
    }
}