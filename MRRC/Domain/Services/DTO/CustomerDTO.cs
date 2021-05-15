namespace MRRC
{
    /// <summary>
    /// Customer data-transfer-object
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerDTO : EntityDTO
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string dateTime { get; set; }

        public CustomerDTO(string ID, string title, string firstName, string lastName, string gender, string dateTime)
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
