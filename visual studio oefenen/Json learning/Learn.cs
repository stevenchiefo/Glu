using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json_learning
{
    internal class Learn
    {
        private Character m_Character;

        public Learn()
        {
            Start();
            Load();
            Test();
            Save();
        }

        private void Start()
        {
        }

        private void Load()
        {
            if (File.Exists("Level.json"))
            {
                using (StreamReader sr = new StreamReader("Level.json"))
                {
                    string content = sr.ReadToEnd();

                    m_Character = JsonConvert.DeserializeObject<Character>(content);

                    Console.WriteLine(m_Character.m_Name);
                    Console.WriteLine(m_Character.m_Leeftijd);
                    Console.WriteLine(m_Character.m_Skill);
                }
            }
        }

        private void Test()
        {
            m_Character = new Character();
            Console.WriteLine("Type uw naam in");
            m_Character.m_Name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Type uw leeftijd in");
            m_Character.m_Leeftijd = Console.ReadLine();
            Console.Clear();
            m_Character.m_Skill = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Press any button to continue");
            Console.ReadKey();
        }

        private void Save()
        {
            using (StreamWriter file = File.CreateText("Level.json"))
            {
                string json = JsonConvert.SerializeObject(m_Character);
                file.Write(json);
            }
        }
    }
}