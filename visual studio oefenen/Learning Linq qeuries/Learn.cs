using System;
using System.Diagnostics;
using System.Xml;

namespace Learning_Linq_qeuries
{
    internal class Learn
    {
        private string m_Skill = "";
        private string m_Leeftijd = "";
        private string m_Zin = "";

        public Learn()
        {
            load();
            Test();
            save();
            //Process.Start("www.google.com");
        }

        private void Test()
        {
            Console.WriteLine("Dit was de vorige Character");
            Console.WriteLine(m_Zin);
            Console.WriteLine(m_Leeftijd);
            Console.WriteLine(m_Skill);
            Console.WriteLine("Voer de naam van je character in");
            string zin = Console.ReadLine();
            Console.WriteLine("Voer je leeftijd in");
            bool valid = false;
            while (!valid)
            {
                string leeftijd = Console.ReadLine();
                int value;

                if (int.TryParse(leeftijd, out value) && value >= 0 && value <= 100)
                {
                    m_Leeftijd = value.ToString();
                    valid = !valid;
                }
                else
                {
                    Console.WriteLine("Not a number");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.WriteLine("Wat is je skill");
            string skill = Console.ReadLine();
            m_Skill = skill;
            m_Zin = zin;
        }

        private void save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("Characters");
            xmlDoc.AppendChild(rootNode);
            XmlNode userNode = xmlDoc.CreateElement("naam");
            XmlAttribute attribute = xmlDoc.CreateAttribute("age");
            attribute.Value = m_Leeftijd;
            userNode.Attributes.Append(attribute);
            XmlAttribute attribute1 = xmlDoc.CreateAttribute("Skill");
            attribute1.Value = m_Skill;
            userNode.Attributes.Append(attribute1);
            rootNode.AppendChild(userNode);
            userNode.InnerText = m_Zin;
            xmlDoc.Save("Test-doc.xml");
        }

        private void load()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Test-doc.xml");
                XmlNodeList Usernodes = xmlDoc.SelectNodes("//Characters/naam");
                foreach (XmlNode usernode in Usernodes)
                {
                    string zin = usernode.InnerText;
                    string Leeftijd = usernode.Attributes["age"].Value.ToString();
                    string skill = usernode.Attributes["Skill"].Value;
                    m_Zin = zin;
                    m_Leeftijd = Leeftijd;
                    m_Skill = skill;
                }
            }
            catch
            {
                Console.Error.WriteLine("Could not find file");
            }
        }
    }
}