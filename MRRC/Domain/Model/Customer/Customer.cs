using System;

namespace MRRC
{
    /// <summary>
    /// Customer entity
    /// Lewis Watson 2020
    /// </summary>
    public class Customer : Entity
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Gender gender { get; set; }
        public DateTime dateTime { get; set; }

        public Customer(int ID, string title, string firstName, string lastName, Gender gender, DateTime dateTime)
        {
            this.ID = ID;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.dateTime = dateTime;
        }
    }
}
