using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace Proj1_SampleConApp
{
    interface IPatientRecord
    {
        void addpatient(int id, string name, int age, string disease);
        void updatePatient(int id, string name, int age, string disease);
        void deletePatients(int id);
        DataTable getPatients();

    }

    class Patient : IPatientRecord
    {
        public DataTable ptable;
        public Patient()
        {
            ptable = new DataTable();
            ptable.Columns.Add("pid", typeof(int));
            ptable.Columns.Add("pname", typeof(string));
            ptable.Columns.Add("page", typeof(int));
            ptable.Columns.Add("pdisease", typeof(string));

            ptable.AcceptChanges();
        }
        public void addpatient(int id, string name, int age, string disease)
        {
            
            DataRow row = ptable.NewRow();
            row[0] = id;
            row[1] = name;
            row[2] = age;
            row[3] = disease;
            ptable.Rows.Add(row);
            ptable.AcceptChanges();
          
          
        }

        public void deletePatients(int id)
        {
            
            foreach (DataRow row in ptable.Rows) 
            {
                
                if (Convert.ToInt32(row[0]) == id) 
                {
                    row.Delete();
                }
            }
            ptable.AcceptChanges();
        }

        public DataTable getPatients()
        {
            Console.WriteLine(ptable);
            return ptable;
        }

        public void updatePatient(int id, string name, int age, string disease)
        {
            foreach (DataRow row in ptable.Rows)
            {
                if (Convert.ToInt32(row[0]) == id)
                {
                    row[1] = name;
                    row[2] = age;
                    row[3] = disease;
                }
            }
            ptable.AcceptChanges();
        }
    }

    class PatientFactory
    {
        public static IPatientRecord GetComponent()
        {
            return new Patient();
        }
    }


   public  class patientRecords
    {
        FileStream fs = File.Create("ptrecs.xml");
        
        const string fileName = @"C:\Users\rrprajna\source\repos\DotnetTraining\Proj1-SampleConApp\Menu.txt";
        static IPatientRecord ptnt = PatientFactory.GetComponent();

       public static void Main(string[] args)
        {


            ptnt.addpatient(1, "Prajna", 22, "Covid 19");
            ptnt.addpatient(2, "Pratham", 45, "Fever");
            ptnt.addpatient(3, "Aarna", 78, "Malaria");
            ptnt.addpatient(4, "John", 33, "Dengue");
            ptnt.addpatient(5, "Serha", 11, "HeartAttack");
            ptnt.addpatient(6, "Vinay", 12, "Cancer");

            string content = File.ReadAllText(fileName);
            var processing = true;
            do
            {
                Console.WriteLine(content);
                string choice = Console.ReadLine();
                processing = processMenu(choice);

            } while (processing);
            ptSerialization();
            //ptDeserialization();
        }

        private static bool processMenu(string choice)
        {
            Patient pt1 = new Patient();
            switch (choice)
            {
                case "A":
                    return addmorePatients();
                    
                case "U":
                    return modifyPatients();
                case "F":
                    return findPatients();
                case "D":

                    return removePatients();
                

                default:
                    return false;
            }
        }

        private static bool findPatients()
        {
            var table = ptnt.getPatients();
            Console.WriteLine("Enter the Id of Patient");
            int id = int.Parse(Console.ReadLine());
            foreach (DataRow row in table.Rows)
            {
                if (row[0].ToString() == id.ToString())
                    Console.WriteLine($"{row[0]} with Name {row["pname"]}");
            }
            return true;
        }

        private static bool addmorePatients()
        {
            Console.WriteLine("Enter patient's id");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter patient's name");
            string name = Console.ReadLine();
            Console.WriteLine("Enter patient's age");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter patient's disease");
            string disease = Console.ReadLine();
            ptnt.addpatient(id, name, age, disease);
            string rec = $"{id},{name},{age},{disease}";
            File.AppendAllText("ptrecs.xml", rec);

            return true;
        }
        private static bool removePatients()
        {
            
            Console.WriteLine("Enter patient's id");
            int id = int.Parse(Console.ReadLine());
            try
            {
                ptnt.deletePatients(id);
                Console.WriteLine("Patient deleted successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
           
        }
        private static bool modifyPatients()
        {
            Console.WriteLine("Enter patient's id");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter patient's updated name");
            string name = Console.ReadLine();
            Console.WriteLine("Enter patient's updated age");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter patient's updated disease");
            string disease = Console.ReadLine();
            ptnt.updatePatient(id,name,age,disease);
            Console.WriteLine($"{id},{name},{age},{disease}");
            return true;
        }
        

        private static void ptSerialization()
        {
            
            FileStream fs = new FileStream("ptrecs.xml", FileMode.OpenOrCreate, FileAccess.Write);
            XmlSerializer xm = new XmlSerializer(typeof(Patient));
            xm.Serialize(fs, ptnt);
            fs.Close();
        }
        private static void ptDeserialization()
        {
            FileStream fs = new FileStream("ptrecs.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xm = new XmlSerializer(typeof(Patient));
            Patient extracted = xm.Deserialize(fs) as Patient;
            Console.WriteLine(extracted.ToString());
        }

       
    }
}

