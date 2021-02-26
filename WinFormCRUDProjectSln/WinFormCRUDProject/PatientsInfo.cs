using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormCRUDProject
{
    public class PatientsInfo
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Diseases { get; set; }
        public int DoctorsId { get; set; }
        public string DoctorsName { get; set; }
        public string Designation { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }
    }
}
